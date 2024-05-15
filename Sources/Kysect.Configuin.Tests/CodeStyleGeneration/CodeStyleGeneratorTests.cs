using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.CodeStyleDoc;
using Kysect.Configuin.CodeStyleDoc.Models;
using Kysect.Configuin.EditorConfig;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Tools;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class CodeStyleGeneratorTests
{
    private readonly MsLearnDocumentationParser _msLearnDocumentationParser = new MsLearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());
    private readonly DotnetConfigSettingsParser _dotnetConfigSettingsParser = new DotnetConfigSettingsParser(TestLogger.ProviderForTests());
    private readonly MsLearnDocumentationInfoLocalReader _repositoryPathReader = TestImplementations.CreateDocumentationInfoLocalProvider();
    private readonly EditorConfigDocumentParser _documentParser;
    private readonly CodeStyleGenerator _sut;

    public CodeStyleGeneratorTests()
    {
        _sut = new CodeStyleGenerator(TestLogger.ProviderForTests());
        _documentParser = new EditorConfigDocumentParser();
    }

    [Fact]
    public void Generate_ForAllMsLearnDocumentation_FinishWithoutErrors()
    {
        string pathToIniFile = Path.Combine("Resources", "Editor-config-sample.ini");

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _repositoryPathReader.Provide(Constants.GetPathToMsDocsRoot());
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        string fileText = File.ReadAllText(pathToIniFile);
        EditorConfigDocument editorConfigDocument = _documentParser.Parse(fileText);
        DotnetConfigSettings dotnetConfigSettings = _dotnetConfigSettingsParser.Parse(editorConfigDocument);

        CodeStyle codeStyle = _sut.Generate(dotnetConfigSettings, roslynRules);

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