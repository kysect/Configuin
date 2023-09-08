using FluentAssertions;
using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.MsLearn;
using Kysect.Configuin.MsLearn.Models;
using Kysect.Configuin.RoslynModels;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class CodeStyleGeneratorTests
{
    private readonly MsLearnDocumentationParser _msLearnDocumentationParser = new MsLearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());
    private readonly EditorConfigSettingsParser _editorConfigSettingsParser = new EditorConfigSettingsParser(TestLogger.ProviderForTests());
    private readonly MsLearnDocumentationInfoLocalProvider _repositoryPathProvider = TestImplementations.CreateDocumentationInfoLocalProvider();
    private readonly CodeStyleGenerator _sut;

    public CodeStyleGeneratorTests()
    {
        _sut = new CodeStyleGenerator(TestLogger.ProviderForTests());
    }

    [Test]
    public void Generate_ForAllMsLearnDocumentation_FinishWithoutErrors()
    {
        string pathToIniFile = Path.Combine("Resources", "Editor-config-sample.ini");

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _repositoryPathProvider.Provide();
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        string fileText = File.ReadAllText(pathToIniFile);
        EditorConfigSettings editorConfigSettings = _editorConfigSettingsParser.Parse(fileText);

        CodeStyle codeStyle = _sut.Generate(editorConfigSettings, roslynRules);

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