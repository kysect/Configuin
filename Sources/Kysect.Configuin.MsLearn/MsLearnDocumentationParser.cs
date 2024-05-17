using Kysect.Configuin.Common;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.MsLearn.Tables;
using Kysect.Configuin.MsLearn.Tables.Models;
using Kysect.Configuin.RoslynModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;
using Microsoft.Extensions.Logging;

namespace Kysect.Configuin.MsLearn;

public class MsLearnDocumentationParser : IMsLearnDocumentationParser
{
    private readonly ILogger _logger;

    private readonly MarkdownTableParser _markdownTableParser;
    private readonly MsLearnTableParser _msLearnTableParser;
    private readonly IMarkdownTextExtractor _textExtractor;
    private readonly MsLearnDocumentationPreprocessor _documentationPreprocessor;

    public MsLearnDocumentationParser(IMarkdownTextExtractor textExtractor, ILogger logger)
    {
        _textExtractor = textExtractor;
        _logger = logger;

        _markdownTableParser = new MarkdownTableParser(textExtractor);
        _msLearnTableParser = new MsLearnTableParser();
        _documentationPreprocessor = new MsLearnDocumentationPreprocessor();
    }

    public RoslynRules Parse(MsLearnDocumentationRawInfo rawInfo)
    {
        ArgumentNullException.ThrowIfNull(rawInfo);

        rawInfo = _documentationPreprocessor.Process(rawInfo);

        _logger.LogInformation("Parsing roslyn rules from MS Learn");

        var roslynQualityRules = rawInfo.QualityRuleFileContents.SelectMany(ParseQualityRules).ToList();
        var roslynStyleRules = rawInfo.StyleRuleFileContents.Select(ParseStyleRules).ToList();

        roslynStyleRules = ParseIde0055FormatOptions(roslynStyleRules, rawInfo);

        return new RoslynRules(roslynQualityRules, roslynStyleRules);
    }

    private List<RoslynStyleRuleGroup> ParseIde0055FormatOptions(
        List<RoslynStyleRuleGroup> roslynStyleRules,
        MsLearnDocumentationRawInfo rawInfo)
    {
        _logger.LogDebug("Parsing IDE0055 options");

        int ruleIde0055Index = roslynStyleRules.FindIndex(r => r.Rules.First().RuleId.Equals(RoslynRuleId.Parse("IDE0055")));
        if (ruleIde0055Index == -1)
        {
            _logger.LogWarning("Rule IDE0055 was not found. Cannot add format options.");
            return roslynStyleRules;
        }

        _logger.LogDebug("Parse dotnet format options");
        IReadOnlyCollection<RoslynStyleRuleOption> dotnetFormattingOptions = ParseAdditionalFormattingOptions(rawInfo.DotnetFormattingOptionsContent);
        _logger.LogDebug("Parse C# format options");
        IReadOnlyCollection<RoslynStyleRuleOption> sharpFormattingOptions = ParseAdditionalFormattingOptions(rawInfo.SharpFormattingOptionsContent);

        var options = new List<RoslynStyleRuleOption>();
        options.AddRange(roslynStyleRules[ruleIde0055Index].Options);
        options.AddRange(dotnetFormattingOptions);
        options.AddRange(sharpFormattingOptions);

        roslynStyleRules[ruleIde0055Index] = roslynStyleRules[ruleIde0055Index] with
        {
            Options = options
        };

        return roslynStyleRules;
    }

    public RoslynStyleRuleGroup ParseStyleRules(string info)
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

        var rules = roslynStyleRuleInformationTables
            .Select(ConvertToRule)
            .ToList();

        return new RoslynStyleRuleGroup(rules, roslynStyleRuleOptions, overviewText, csharpCodeSample);
    }

    private RoslynStyleRuleInformationTable ParseInformationTable(Table tableBlock)
    {
        MarkdownTableContent markdownTableContent = _markdownTableParser.ParseToSimpleContent(tableBlock);
        MsLearnPropertyValueDescriptionTable table = _msLearnTableParser.Parse(markdownTableContent);
        return RoslynStyleRuleInformationTable.Create(table);
    }

    private RoslynStyleRule ConvertToRule(RoslynStyleRuleInformationTable roslynStyleRuleInformationTable)
    {
        return new RoslynStyleRule(
            roslynStyleRuleInformationTable.RuleId,
            roslynStyleRuleInformationTable.Title,
            roslynStyleRuleInformationTable.Category);
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

        IReadOnlyCollection<RoslynRuleId> ruleIds = RoslynRuleIdRange.Parse(ruleId.Value).Enumerate().ToList();

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

        return ConvertBlockToText(headedBlock);
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

        return ConvertBlockToText(overviewBlock);
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

    private string ConvertBlockToText(MarkdownHeadedBlock block)
    {
        return block
            .Content
            .Select(_textExtractor.ExtractText)
            .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
    }
}