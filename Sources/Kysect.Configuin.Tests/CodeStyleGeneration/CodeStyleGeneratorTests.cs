using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.RoslynRuleModels;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Core.MarkdownParsing.TextExtractor;
using Kysect.Configuin.Tests.Tools;
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
        _msLearnDocumentationParser = new MsLearnDocumentationParser(PlainTextExtractor.Create());

        string pathToRoot = Constants.GetPathToMsDocsRoot();

        _repositoryPathProvider = new MsLearnDocumentationInfoLocalProvider(pathToRoot);
    }

    [Test]
    public void Generate_ForAllMsLearnDocumentation_FinishWithoutErrors()
    {
        string pathToIniFile = Path.Combine("Resources", "Editor-config-sample.ini");
        var sut = new CodeStyleGenerator();

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _repositoryPathProvider.Provide();
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        string fileText = File.ReadAllText(pathToIniFile);
        EditorConfigRuleSet editorConfigRuleSet = _editorConfigRuleParser.Parse(fileText);

        CodeStyleInfo codeStyleInfo = sut.Generate(editorConfigRuleSet, roslynRules);

        // TODO: add asserts
        Assert.Pass();
    }
}