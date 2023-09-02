using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.RoslynRuleModels;
using Kysect.Configuin.Core.EditorConfigParsing;
using Kysect.Configuin.Core.CodeStyleGeneration;
using Kysect.Configuin.Core.CodeStyleGeneration.Models;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.CodeStyleGeneration;

public class CodeStyleGeneratorTests
{
    private readonly MsLearnDocumentationParser _msLearnDocumentationParser = new(TestImplementations.GetTextExtractor());
    private readonly EditorConfigSettingsParser _editorConfigSettingsParser = new();
    private readonly MsLearnDocumentationInfoLocalProvider _repositoryPathProvider = TestImplementations.CreateDocumentationInfoLocalProvider();

    [Test]
    public void Generate_ForAllMsLearnDocumentation_FinishWithoutErrors()
    {
        string pathToIniFile = Path.Combine("Resources", "Editor-config-sample.ini");
        var sut = new CodeStyleGenerator();

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = _repositoryPathProvider.Provide();
        RoslynRules roslynRules = _msLearnDocumentationParser.Parse(msLearnDocumentationRawInfo);
        string fileText = File.ReadAllText(pathToIniFile);
        EditorConfigSettings editorConfigSettings = _editorConfigSettingsParser.Parse(fileText);

        CodeStyle codeStyle = sut.Generate(editorConfigSettings, roslynRules);

        // TODO: add asserts
        Assert.Pass();
    }
}