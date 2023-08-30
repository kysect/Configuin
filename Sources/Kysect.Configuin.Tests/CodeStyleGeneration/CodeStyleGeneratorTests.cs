using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.RoslynRuleModels;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.MarkdownParsing;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class CodeStyleGeneratorTests
{
    private readonly MsLearnDocumentationParser _msLearnDocumentationParser;
    private readonly EditorConfigRuleParser _editorConfigRuleParser;
    private readonly MsLearnDocumentationInfoLocalProvider _repositoryPathProvider;

    public CodeStyleGeneratorTests()
    {
        _editorConfigRuleParser = new EditorConfigRuleParser();
        _msLearnDocumentationParser = new MsLearnDocumentationParser(new RoundtripRendererTextExtractor(MarkdownPipelineProvider.GetDefault()));

        // TODO: remove duplication
        string pathToRoot = Path.Combine(
            "..", // netX.0
            "..", // Debug
            "..", // bin
            "..", // Kysect.Configuin.Tests
            "..", // root
            "ms-learn");

        _repositoryPathProvider = new MsLearnDocumentationInfoLocalProvider(pathToRoot);
    }

    // TODO: remove ignore
    [Test]
    [Ignore("Return to this test after fixes in _msLearnDocumentationParser.Parse")]
    public void Generate_ForAllMsLearnDocumentation_FinishWithoutErrors()
    {
        string fileText = File.ReadAllText(Path.Combine("EditorConfigFileParsing", "Resources", "Editor-config-sample.ini"));
        var sut = new CodeStyleGenerator();

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _repositoryPathProvider.Provide();
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        EditorConfigRuleSet editorConfigRuleSet = _editorConfigRuleParser.Parse(fileText);

        CodeStyleInfo codeStyleInfo = sut.Generate(editorConfigRuleSet, roslynRules);

        // TODO: add asserts
        Assert.Pass();
    }
}