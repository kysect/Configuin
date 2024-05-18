using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;
using Kysect.Configuin.Markdown.Documents;
using Kysect.Configuin.Markdown.Tables;
using Kysect.Configuin.Markdown.Tables.Models;
using Kysect.Configuin.Markdown.TextExtractor;
using Kysect.Configuin.RoslynModels;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace Kysect.Configuin.Learn.ContentParsing;

public class LearnMarkdownBlockParser(IMarkdownTextExtractor textExtractor, MarkdownTableParser markdownTableParser, LearnTableParser learnTableParser)
{
    public IReadOnlyCollection<RoslynStyleRuleOption> ParseOptions(IReadOnlyCollection<MarkdownHeadedBlock> markdownHeadedBlocks)
    {
        return markdownHeadedBlocks
            .Where(HeaderForOption)
            .Select(ParseOption)
            .ToList();
    }

    private bool HeaderForOption(MarkdownHeadedBlock markdownHeadedBlock)
    {
        markdownHeadedBlock.ThrowIfNull();

        // TODO: do it in better way? Need to parse list of options
        string headerText = markdownHeadedBlock.HeaderText;

        return headerText.StartsWith("dotnet_")
               || headerText.StartsWith("csharp_")
               // IDE0073
               || headerText == "file_header_template";
    }

    private RoslynStyleRuleOption ParseOption(MarkdownHeadedBlock optionBlock)
    {
        optionBlock.ThrowIfNull();

        var tables = optionBlock.Content.OfType<Table>().ToList();
        if (tables.Count != 1)
            throw new ConfiguinException($"Unexpected table count in option block {optionBlock.HeaderText}");

        MarkdownTableContent markdownTableContent = markdownTableParser.ParseToSimpleContent(tables.Single());
        LearnPropertyValueDescriptionTable table = learnTableParser.Parse(markdownTableContent);

        string? csharpCodeSample = TryExtractCsharpCodeBlock(optionBlock);

        LearnPropertyValueDescriptionTableRow optionName = table.GetSingleValue("Option name");
        IReadOnlyList<LearnPropertyValueDescriptionTableRow> optionValues = table.FindValues("Option values");
        LearnPropertyValueDescriptionTableRow? defaultValue = table.FindValues("Default option value").SingleOrDefault();

        return new RoslynStyleRuleOption(
            optionName.Value,
            optionValues.Select(v => new RoslynStyleRuleOptionValue(v.Value, v.Description)).ToList(),
            defaultValue?.Value,
            csharpCodeSample);
    }

    public string? TryExtractCsharpCodeBlock(MarkdownHeadedBlock block)
    {
        block.ThrowIfNull();

        FencedCodeBlock? codeBlock = block.Content
            .OfType<FencedCodeBlock>()
            .FirstOrDefault(cb => cb.Info == "csharp");

        if (codeBlock is null)
            return null;

        return textExtractor.ExtractText(codeBlock);
    }

    public string ConvertBlockToText(MarkdownHeadedBlock block)
    {
        block.ThrowIfNull();

        return block
            .Content
            .Select(textExtractor.ExtractText)
            .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
    }
}