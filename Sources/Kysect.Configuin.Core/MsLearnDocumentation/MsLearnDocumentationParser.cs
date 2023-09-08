using Kysect.Configuin.Common;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.Core.MsLearnDocumentation;

public class MsLearnDocumentationParser : IMsLearnDocumentationParser
{
    private readonly ILogger _logger;

    private readonly MarkdownTableParser _markdownTableParser;
    private readonly MsLearnTableParser _msLearnTableParser;
    private readonly IMarkdownTextExtractor _textExtractor;

    public MsLearnDocumentationParser(IMarkdownTextExtractor textExtractor, ILogger logger)
    {
        _textExtractor = textExtractor;
        _logger = logger;

        _markdownTableParser = new MarkdownTableParser(textExtractor);
        _msLearnTableParser = new MsLearnTableParser();
    }

    public RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo)
    {
        _logger.LogInformation("Parsing roslyn rules from MS Learn");

        IReadOnlyCollection<RoslynStyleRuleOption> dotnetFormattingOptions = ParseAdditionalFormattingOptions(rawInfo.DotnetFormattingOptionsContent);
        IReadOnlyCollection<RoslynStyleRuleOption> sharpFormattingOptions = ParseAdditionalFormattingOptions(rawInfo.SharpFormattingOptionsContent);

        return new RoslynRules(
            rawInfo.QualityRuleFileContents.SelectMany(ParseQualityRules).ToList(),
            rawInfo.StyleRuleFileContents.SelectMany(ParseStyleRules).ToList(),
            dotnetFormattingOptions,
            sharpFormattingOptions);
    }

    public IReadOnlyCollection<RoslynStyleRule> ParseStyleRules(string info)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(info);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(_textExtractor);

        if (markdownHeadedBlocks.Count == 0)
            throw new ConfiguinException("Style rule markdown file does not contains any heading blocks. Cannot parse description");

        var ruleDescriptionTables = markdownHeadedBlocks
            .First()
            .Content
            .OfType<Table>()
            .ToList();

        if (ruleDescriptionTables.Count == 0)
            throw new ConfiguinException($"Style rule description block does not contains child table.");

        var roslynStyleRuleInformationTables = ruleDescriptionTables
            .Select(ParseInformationTable)
            .ToList();

        string overviewText = GetStyleOverviewText(markdownHeadedBlocks);
        string? csharpCodeSample = FindIdeExample(markdownHeadedBlocks);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = ParseOptions(markdownHeadedBlocks);

        return roslynStyleRuleInformationTables
            .Select(table => ConvertToRule(table, overviewText, csharpCodeSample, roslynStyleRuleOptions))
            .ToList();
    }

    private RoslynStyleRuleInformationTable ParseInformationTable(Table tableBlock)
    {
        MarkdownTableContent markdownTableContent = _markdownTableParser.ParseToSimpleContent(tableBlock);
        MsLearnPropertyValueDescriptionTable table = _msLearnTableParser.Parse(markdownTableContent);

        MsLearnPropertyValueDescriptionTableRow ruleId = table.GetSingleValue("Rule ID");
        MsLearnPropertyValueDescriptionTableRow title = table.GetSingleValue("Title");
        MsLearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        MsLearnPropertyValueDescriptionTableRow subcategory = table.GetSingleValue("Subcategory");
        MsLearnPropertyValueDescriptionTableRow applicableLanguages = table.GetSingleValue("Applicable languages");
        // TODO: return as optional parameter. Not all rules contains it
        //MsLearnPropertyValueDescriptionTableRow introducedVersion = table.GetSingleValue("Introduced version");
        var options = table
            .FindValues("Options")
            .Select(o => o.Value)
            .ToList();

        return new RoslynStyleRuleInformationTable(
            RoslynRuleId.Parse(ruleId.Value),
            title.Value,
            category.Value,
            subcategory.Value,
            applicableLanguages.Value,
            options);
    }

    private RoslynStyleRule ConvertToRule(
        RoslynStyleRuleInformationTable roslynStyleRuleInformationTable,
        string overviewText,
        string? example,
        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions)
    {
        return new RoslynStyleRule(
            roslynStyleRuleInformationTable.RuleId,
            roslynStyleRuleInformationTable.Title,
            roslynStyleRuleInformationTable.Category,
            overviewText,
            example: example,
            roslynStyleRuleOptions);
    }

    public IReadOnlyCollection<RoslynQualityRule> ParseQualityRules(string info)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(info);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(_textExtractor);

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
        MsLearnPropertyValueDescriptionTableRow title = table.GetSingleValue("Title");
        MsLearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        // TODO: add this fields to model
        MsLearnPropertyValueDescriptionTableRow breakingChanges = table.GetSingleValue("Fix is breaking or non-breaking");
        // TODO: remove hardcoded dotnet version
        // TODO: docs contains both .NET7 and .NET8 =_=
        //MsLearnPropertyValueDescriptionTableRow isDefault = table.GetSingleValue("Enabled by default in .NET 8");

        IReadOnlyCollection<RoslynRuleId> ruleIds = ParseQualityRuleTableIdRow(ruleId);

        string description = ParseCaRuleDescription(markdownHeadedBlocks);

        return ruleIds
            .Select(id => new RoslynQualityRule(
                id,
                title.Value,
                category.Value,
                description))
            .ToList();
    }

    public IReadOnlyCollection<RoslynStyleRuleOption> ParseAdditionalFormattingOptions(string dotnetFormattingFileContent)
    {
        MarkdownDocument markdownDocument = MarkdownDocumentExtensions.CreateFromString(dotnetFormattingFileContent);
        IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks = markdownDocument.SplitByHeaders(_textExtractor);
        return ParseOptions(markdownHeadedBlocks);
    }

    private string ParseCaRuleDescription(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? headedBlock = markdownHeadedBlocks.FirstOrDefault(b => b.HeaderText == "Rule description");
        if (headedBlock is null)
            throw new ConfiguinException("Quality rule page does not contains Rule description block.");

        string overviewText = headedBlock
            .Content
            .Select(_textExtractor.ExtractText)
            .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");

        return overviewText;
    }

    private IReadOnlyCollection<RoslynRuleId> ParseQualityRuleTableIdRow(MsLearnPropertyValueDescriptionTableRow ruleId)
    {
        if (ruleId.Value.Contains("-"))
            return RoslynRuleIdRange.Parse(ruleId.Value).Enumerate().ToList();

        return new[] { RoslynRuleId.Parse(ruleId.Value) };
    }

    private string GetStyleOverviewText(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? overviewBlock = markdownHeadedBlocks.FirstOrDefault(h => h.HeaderText == "Overview");
        if (overviewBlock is null)
        {
            // TODO: Rule IDE0055 does not contains this block
            //throw new ConfiguinException("Style rule page does not contains Overview block.");

            _logger.LogWarning("Skip overview parsing for IDE0055");
            return string.Empty;
        }

        string overviewText = overviewBlock
            .Content
            .Select(_textExtractor.ExtractText)
            .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");

        return overviewText;
    }

    private string? FindIdeExample(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        MarkdownHeadedBlock? exampleBlock = markdownHeadedBlocks.FirstOrDefault(h => h.HeaderText == "Example");
        if (exampleBlock is null)
            return null;

        return TryExtractCsharpCodeBlock(exampleBlock);
    }

    private IReadOnlyCollection<RoslynStyleRuleOption> ParseOptions(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        return markdownHeadedBlocks
            .Where(HeaderForOption)
            .Select(ParseOption)
            .ToList();
    }

    private bool HeaderForOption(MarkdownHeadedBlock markdownHeadedBlock)
    {
        // TODO: do it in better way?
        string headerText = markdownHeadedBlock.HeaderText;

        return headerText.StartsWith("dotnet_")
               || headerText.StartsWith("csharp_")
               // IDE0073
               || headerText == "file_header_template";
    }

    private RoslynStyleRuleOption ParseOption(MarkdownHeadedBlock optionBlock)
    {
        var tables = optionBlock.Content.OfType<Table>().ToList();
        if (tables.Count != 1)
            throw new ConfiguinException($"Unexpected table count in option block {optionBlock.HeaderText}");

        MarkdownTableContent markdownTableContent = _markdownTableParser.ParseToSimpleContent(tables.Single());
        MsLearnPropertyValueDescriptionTable table = _msLearnTableParser.Parse(markdownTableContent);

        string? csharpCodeSample = TryExtractCsharpCodeBlock(optionBlock);

        MsLearnPropertyValueDescriptionTableRow optionName = table.GetSingleValue("Option name");
        IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> optionValues = table.FindValues("Option values");
        MsLearnPropertyValueDescriptionTableRow? defaultValue = table.FindValues("Default option value").SingleOrDefault();

        return new RoslynStyleRuleOption(
            optionName.Value,
            optionValues.Select(v => new RoslynStyleRuleOptionValue(v.Value, v.Description)).ToList(),
            defaultValue?.Value,
            csharpCodeSample);
    }

    private string? TryExtractCsharpCodeBlock(MarkdownHeadedBlock block)
    {
        FencedCodeBlock? codeBlock = block.Content
            .OfType<FencedCodeBlock>()
            .FirstOrDefault(cb => cb.Info == "csharp");

        if (codeBlock is null)
            return null;

        return _textExtractor.ExtractText(codeBlock);
    }
}