using FluentAssertions;
using Kysect.Configuin.Core.MsLearnDocumentation;
using Kysect.Configuin.Core.MsLearnDocumentation.Models;
using Kysect.Configuin.Core.RoslynRuleModels;
using Kysect.Configuin.Tests.Resources;
using Kysect.Configuin.Tests.Tools;
using NUnit.Framework;

namespace Kysect.Configuin.Tests.MsLearnDocumentation;

public class MsLearnDocumentationParserTests
{
    private static readonly MsLearnRepositoryPathProvider MsLearnRepositoryPathProvider = TestImplementations.CreateRepositoryPathProvider();

    private readonly MsLearnDocumentationParser _parser = new MsLearnDocumentationParser(TestImplementations.GetTextExtractor(), TestLogger.ProviderForTests());

    [Test]
    public void ParseStyleRule_IDE0001_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0001.md");

        RoslynStyleRule expected = WellKnownRoslynRuleDefinitions.IDE0001();

        IReadOnlyCollection<RoslynStyleRule> roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseStyleRule_IDE0040_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0040.md");

        RoslynStyleRule expected = WellKnownRoslynRuleDefinitions.IDE0040();

        IReadOnlyCollection<RoslynStyleRule> roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseStyleRule_IDE0003_0009_ReturnExpectedResult()
    {
        string fileText = GetIdeDescription("ide0003-ide0009.md");

        var options = new RoslynStyleRuleOption[]
        {
            // TODO: add dots to end of lines into source code and fix test
            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_field",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer fields to be prefaced with this. in C# or Me. in Visual Basic"),
                    new RoslynStyleRuleOptionValue("false", "Prefer fields not to be prefaced with this. or Me."),
                },
                "false",
                """
                // dotnet_style_qualification_for_field = true
                this.capacity = 0;
                
                // dotnet_style_qualification_for_field = false
                capacity = 0;
                """),

            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_property",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer properties to be prefaced with this. in C# or Me. in Visual Basic."),
                    new RoslynStyleRuleOptionValue("false", "Prefer properties not to be prefaced with this. or Me.."),
                },
                "false",
                """
                // dotnet_style_qualification_for_property = true
                this.ID = 0;
                
                // dotnet_style_qualification_for_property = false
                ID = 0;
                """),

            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_method",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer methods to be prefaced with this. in C# or Me. in Visual Basic."),
                    new RoslynStyleRuleOptionValue("false", "Prefer methods not to be prefaced with this. or Me.."),
                },
                "false",
                """
                // dotnet_style_qualification_for_method = true
                this.Display();
                
                // dotnet_style_qualification_for_method = false
                Display();
                """),

            new RoslynStyleRuleOption(
                "dotnet_style_qualification_for_event",
                new []
                {
                    new RoslynStyleRuleOptionValue("true", "Prefer events to be prefaced with this. in C# or Me. in Visual Basic."),
                    new RoslynStyleRuleOptionValue("false", "Prefer events not to be prefaced with this. or Me.."),
                },
                "false",
                """
                // dotnet_style_qualification_for_event = true
                this.Elapsed += Handler;
                
                // dotnet_style_qualification_for_event = false
                Elapsed += Handler;
                """),
        };

        string overview = """
                          These two rules define whether or not you prefer the use of this (C#) and Me. (Visual Basic) qualifiers. To enforce that the qualifiers aren't present, set the severity of IDE0003 to warning or error. To enforce that the qualifiers are present, set the severity of IDE0009 to warning or error.
                          For example, if you prefer qualifiers for fields and properties but not for methods or events, then you can enable IDE0009 and set the options dotnet_style_qualification_for_field and dotnet_style_qualification_for_property to true. However, this configuration would not flag methods and events that do have this and Me qualifiers. To also enforce that methods and events don't have qualifiers, enable IDE0003.
                          """;

        var ide0003 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0003"),
            "Remove this or Me qualification",
            "Style",
            overview,
            null,
            options);

        var ide0009 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0009"),
            "Add this or Me qualification",
            "Style",
            overview,
            null,
            options);

        IReadOnlyCollection<RoslynStyleRule> roslynStyleRules = _parser.ParseStyleRules(fileText);

        roslynStyleRules.Should().HaveCount(2);
        roslynStyleRules.ElementAt(0).Should().BeEquivalentTo(ide0003);
        roslynStyleRules.ElementAt(1).Should().BeEquivalentTo(ide0009);
    }

    [Test]
    public void ParseQualityRule_CA1064_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1064.md");

        RoslynQualityRule expected = WellKnownRoslynRuleDefinitions.CA1064();

        IReadOnlyCollection<RoslynQualityRule> qualityRule = _parser.ParseQualityRules(fileText);

        qualityRule.Should().HaveCount(1)
            .And.Subject.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ParseQualityRule_CA1865_CA1867_ReturnExpectedResult()
    {
        string fileText = GetPathToCa("ca1865-ca1867.md");

        // TODO: parse description
        var expected = new RoslynQualityRule(
            RoslynRuleId.Parse("CA1865"),
            "Use 'string.Method(char)' instead of 'string.Method(string)' for string with single char",
            category: "Performance",
            // TODO: parse description?
            description: string.Empty);

        IReadOnlyCollection<RoslynQualityRule> qualityRules = _parser.ParseQualityRules(fileText);

        qualityRules.Should().HaveCount(3);
        qualityRules.ElementAt(0).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Parse_DotnetFormattingOptions_ReturnExpectedResult()
    {
        string pathToDotnetFormattingFile = MsLearnRepositoryPathProvider.GetPathToDotnetFormattingFile();
        string fileContent = File.ReadAllText(pathToDotnetFormattingFile);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        var dotnet_sort_system_directives_first = new RoslynStyleRuleOption(
            "dotnet_sort_system_directives_first",
            new[]
            {
                new RoslynStyleRuleOptionValue("true", "Sort System.* using directives alphabetically, and place them before other using directives."),
                new RoslynStyleRuleOptionValue("false", "Do not place System.* using directives before other using directives.")
            },
            "true",
            """
            // dotnet_sort_system_directives_first = true
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using Octokit;
            
            // dotnet_sort_system_directives_first = false
            using System.Collections.Generic;
            using Octokit;
            using System.Threading.Tasks;
            """);

        var dotnet_separate_import_directive_groups = new RoslynStyleRuleOption(
            "dotnet_separate_import_directive_groups",
            new[]
            {
                new RoslynStyleRuleOptionValue("true", "Place a blank line between using directive groups."),
                new RoslynStyleRuleOptionValue("false", "Do not place a blank line between using directive groups.")
            },
            "false",
            """
            // dotnet_separate_import_directive_groups = true
            using System.Collections.Generic;
            using System.Threading.Tasks;
            
            using Octokit;
            
            // dotnet_separate_import_directive_groups = false
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using Octokit;
            """);


        roslynStyleRuleOptions.Should().HaveCount(2);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(dotnet_sort_system_directives_first);
        roslynStyleRuleOptions.ElementAt(1).Should().BeEquivalentTo(dotnet_separate_import_directive_groups);
    }

    [Test]
    public void Parse_CsharpFormattingOptions_ReturnExpectedResult()
    {
        string pathToFile = MsLearnRepositoryPathProvider.GetPathToSharpFormattingFile();
        string fileContent = File.ReadAllText(pathToFile);
        var csharp_new_line_before_open_brace = new RoslynStyleRuleOption(
            "csharp_new_line_before_open_brace",
            new[]
            {
                new RoslynStyleRuleOptionValue("all", "Require braces to be on a new line for all expressions (\"Allman\" style)."),
                new RoslynStyleRuleOptionValue("none", "Require braces to be on the same line for all expressions (\"K&R\")."),
                new RoslynStyleRuleOptionValue("accessors, anonymous_methods, anonymous_types, control_blocks, events, indexers,lambdas, local_functions, methods, object_collection_array_initializers, properties, types", "Require braces to be on a new line for the specified code element (\"Allman\" style)."),
            },
            "all",
            """
            // csharp_new_line_before_open_brace = all
            void MyMethod()
            {
                if (...)
                {
                    ...
                }
            }
            
            // csharp_new_line_before_open_brace = none
            void MyMethod() {
                if (...) {
                    ...
                }
            }
            """);

        IReadOnlyCollection<RoslynStyleRuleOption> roslynStyleRuleOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        roslynStyleRuleOptions.Should().HaveCount(37);
        roslynStyleRuleOptions.ElementAt(0).Should().BeEquivalentTo(csharp_new_line_before_open_brace);
    }

    [Test]
    [Ignore("Issue #33")]
    public void Parse_CodeStyleRefactoringOptions_ReturnExpectedResult()
    {
        string pathToFile = string.Empty;
        string fileContent = File.ReadAllText(pathToFile);
        var dotnet_style_operator_placement_when_wrapping = new RoslynStyleRuleOption(
            "dotnet_style_operator_placement_when_wrapping",
            new[]
            {
                new RoslynStyleRuleOptionValue("end_of_line", "Place operator at the end of a line."),
                new RoslynStyleRuleOptionValue("beginning_of_line", "Place operator on a new line."),
            },
            "beginning_of_line",
            """
            // dotnet_style_operator_placement_when_wrapping = end_of_line
            if (true && 
                true)
            
            // dotnet_style_operator_placement_when_wrapping = beginning_of_line
            if (true
                && true)
            """);

        IReadOnlyCollection<RoslynStyleRuleOption> codeStyleRefactoringOptions = _parser.ParseAdditionalFormattingOptions(fileContent);

        codeStyleRefactoringOptions.Should().HaveCount(1);
        codeStyleRefactoringOptions.ElementAt(0).Should().BeEquivalentTo(dotnet_style_operator_placement_when_wrapping);
    }

    // TODO: remove ignore
    [Test]
    [Ignore("Need to fix all related problems")]
    public void Parse_MsDocsRepository_FinishWithoutError()
    {
        MsLearnDocumentationInfoLocalProvider repositoryPathProvider = TestImplementations.CreateDocumentationInfoLocalProvider();

        MsLearnDocumentationRawInfo msLearnDocumentationRawInfo = repositoryPathProvider.Provide();
        RoslynRules roslynRules = _parser.Parse(msLearnDocumentationRawInfo);

        // TODO: add asserts
        Assert.Pass();
    }

    private static string GetIdeDescription(string fileName)
    {
        string path = Path.Combine(MsLearnRepositoryPathProvider.GetPathToStyleRules(), fileName);
        return File.ReadAllText(path);
    }

    private static string GetPathToCa(string fileName)
    {
        string path = Path.Combine(MsLearnRepositoryPathProvider.GetPathToQualityRules(), fileName);
        return File.ReadAllText(path);
    }
}