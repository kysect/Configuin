using Kysect.Configuin.Core.MarkdownParsing.Documents;
using Kysect.Configuin.Core.MarkdownParsing.Tables;
using Kysect.Configuin.Core.MarkdownParsing.Tables.Models;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Core.MsLearnDocumentation;

public class MsLearnDocumentationParser : IMsLearnDocumentationParser
{
    private readonly MarkdownTableParser _markdownTableParser;
    private readonly MsLearnTableParser _msLearnTableParser;
    private readonly IMarkdownTextExtractor _textExtractor;

    public MsLearnDocumentationParser(IMarkdownTextExtractor textExtractor)
    {
        _textExtractor = textExtractor;
        _markdownTableParser = new MarkdownTableParser(textExtractor);
        _msLearnTableParser = new MsLearnTableParser();
    }

    public RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo)
    {
        // TODO: implement parsing for other info - SharpFormattingOptionsContent and DotnetFormattingOptionsContent

        return new RoslynRules(
            rawInfo.QualityRuleInfos.Select(ParseQualityRule).ToList(),
            rawInfo.StyleRuleInfos.Select(ParseStyleRule).ToList());
    }

    public IReadOnlyCollection<RoslynStyleRule> ParseStyleRules(string info)
    {
        // TODO: do in better way
        return new[] { ParseStyleRule(info) };
    }

    public RoslynStyleRule ParseStyleRule(string info)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(info);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders();

        if (markdownHeadedBlocks.Count == 0)
            throw new ConfiguinException("Style rule markdown file does not contains any heading blocks. Cannot parse description");

        MarkdownHeadedBlock markdownHeadedBlock = markdownHeadedBlocks.First();
        if (markdownHeadedBlock.Content.Count != 1)
            throw new ConfiguinException($"Style rule description block contains unexpected child count. Expected 1, but was {markdownHeadedBlock.Content.Count}");

        Block block = markdownHeadedBlock.Content.Single();
        if (block is not Table tableBlock)
            throw new ConfiguinException($"Style rule description block must contains Table block but was {block.GetType()}");

        MarkdownTableContent markdownTableContent = _markdownTableParser.ParseToSimpleContent(tableBlock);
        MsLearnPropertyValueDescriptionTable table = _msLearnTableParser.Parse(markdownTableContent);

        MsLearnPropertyValueDescriptionTableRow ruleId = table.GetSingleValue("Rule ID");
        MsLearnPropertyValueDescriptionTableRow title = table.GetSingleValue("Title");
        MsLearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        MsLearnPropertyValueDescriptionTableRow subcategory = table.GetSingleValue("Subcategory");
        MsLearnPropertyValueDescriptionTableRow applicableLanguages = table.GetSingleValue("Applicable languages");
        // TODO: return as optional parameter. Not all rules contains it
        //MsLearnPropertyValueDescriptionTableRow introducedVersion = table.GetSingleValue("Introduced version");
        IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> options = table.FindValues("Options");

        string overviewText = GetStyleOverviewText(markdownHeadedBlocks);
        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = ParseOptions(markdownHeadedBlocks);

        return new RoslynStyleRule(
            RoslynRuleId.Parse(ruleId.Value),
            title.Value,
            category.Value,
            overviewText,
            example: string.Empty,
            roslynStyleRuleOptions);
    }

    public RoslynQualityRule ParseQualityRule(string info)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(info);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders();

        if (markdownHeadedBlocks.Count == 0)
            throw new ConfiguinException("Style rule markdown file does not contains any heading blocks. Cannot parse description");

        MarkdownHeadedBlock markdownHeadedBlock = markdownHeadedBlocks.First();
        IReadOnlyCollection<Table> contentBlocks = markdownHeadedBlock.Content.OfType<Table>().ToList();
        if (contentBlocks.Count != 1)
            throw new ConfiguinException($"Style rule description block contains unexpected child count. Expected 1, but was {contentBlocks.Count}");

        Table tableBlock = contentBlocks.Single();
        MarkdownTableContent markdownTableContent = _markdownTableParser.ParseToSimpleContent(tableBlock);
        MsLearnPropertyValueDescriptionTable table = _msLearnTableParser.Parse(markdownTableContent);

        MsLearnPropertyValueDescriptionTableRow ruleId = table.GetSingleValue("Rule ID");
        MsLearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        // TODO: add this fields to model
        MsLearnPropertyValueDescriptionTableRow breakingChanges = table.GetSingleValue("Fix is breaking or non-breaking");
        // TODO: remove hardcoded dotnet version
        // TODO: docs contains both .NET7 and .NET8 =_=
        //MsLearnPropertyValueDescriptionTableRow isDefault = table.GetSingleValue("Enabled by default in .NET 8");


        return new RoslynQualityRule(
            RoslynRuleId.Parse(ruleId.Value),
            // TODO: parse rule name
            ruleName: string.Empty,
            category.Value,
            // TODO: parse description
            description: string.Empty);
    }

    private string GetStyleOverviewText(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? overviewBlock = markdownHeadedBlocks.FirstOrDefault(h => h.GetHeaderText() == "Overview");
        if (overviewBlock is null)
            throw new ConfiguinException("Style rule page does not contains Overview block.");

        string overviewText = overviewBlock
            .Content
            .Select(_textExtractor.ExtractText)
            .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");

        return overviewText;
    }

    private IReadOnlyCollection<RoslynStyleRuleOption> ParseOptions(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        // TODO: fix header with option detecting

        return markdownHeadedBlocks
            .Where(h => h.GetHeaderText().StartsWith("dotnet_"))
            .Select(ParseOption)
            .ToList();
    }

    private RoslynStyleRuleOption ParseOption(MarkdownHeadedBlock optionBlock)
    {
        var tables = optionBlock.Content.OfType<Table>().ToList();
        if (tables.Count != 1)
            throw new ConfiguinException($"Unexpected table count in option block {optionBlock.GetHeaderText()}");

        MarkdownTableContent markdownTableContent = _markdownTableParser.ParseToSimpleContent(tables.Single());
        MsLearnPropertyValueDescriptionTable table = _msLearnTableParser.Parse(markdownTableContent);

        var codeBlocks = optionBlock.Content.OfType<CodeBlock>().ToList();
        CodeBlock? csharpCodeBlock = codeBlocks
            .OfType<FencedCodeBlock>()
            .FirstOrDefault(cb => cb.Info == "csharp");
        // TODO: use null instead of empty line
        string csharpCodeSample = csharpCodeBlock is null
                                    ? ""
                                    : _textExtractor.ExtractText(csharpCodeBlock);

        MsLearnPropertyValueDescriptionTableRow optionName = table.GetSingleValue("Option name");
        IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> optionValues = table.FindValues("Option values");
        MsLearnPropertyValueDescriptionTableRow defaultValue = table.GetSingleValue("Default option value");

        return new RoslynStyleRuleOption(
            optionName.Value,
            optionValues.Select(v => new RoslynStyleRuleOptionValue(v.Value, v.Description)).ToList(),
            defaultValue.Value,
            csharpCodeSample);
    }
}