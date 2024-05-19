using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.Learn;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class CodeStyleGeneratorTests
{
    private readonly EditorConfigDocumentParser _documentParser;
    private readonly LearnDocumentationParser _learnDocumentationParser;
    private readonly CodeStyleGenerator _sut;

    public CodeStyleGeneratorTests()
    {
        _sut = new CodeStyleGenerator(TestLogger.ProviderForTests());
        _documentParser = new EditorConfigDocumentParser();

        _learnDocumentationParser = new LearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());
    }

    [Fact]
    public void Generate_ForAllMsLearnDocumentation_FinishWithoutErrors()
    {
        string pathToIniFile = Path.Combine("Resources", "Editor-config-sample.ini");

        RoslynRules roslynRules = _learnDocumentationParser.Parse(Constants.GetPathToMsDocsRoot());
        string fileText = File.ReadAllText(pathToIniFile);
        EditorConfigDocument editorConfigDocument = _documentParser.Parse(fileText);
        CodeStyle codeStyle = _sut.Generate(editorConfigDocument, roslynRules);

        ICodeStyleElement codeStyleElement = codeStyle.Elements.ElementAt(2);
        codeStyleElement.Should().BeOfType<CodeStyleRoslynStyleRuleConfiguration>();
        CodeStyleRoslynStyleRuleConfiguration roslynStyleRuleConfiguration = codeStyleElement.To<CodeStyleRoslynStyleRuleConfiguration>();
        roslynStyleRuleConfiguration.Severity.Should().Be(RoslynRuleSeverity.Warning);
        roslynStyleRuleConfiguration.Rule.RuleId.Should().Be(RoslynRuleId.Parse("IDE0003"));
        roslynStyleRuleConfiguration.Options.Should().HaveCount(4);
        roslynStyleRuleConfiguration.Options.ElementAt(0).Option.Name.Should().Be("dotnet_style_qualification_for_field");
        roslynStyleRuleConfiguration.Options.ElementAt(0).SelectedValue.Should().Be("false:warning");
    }
}