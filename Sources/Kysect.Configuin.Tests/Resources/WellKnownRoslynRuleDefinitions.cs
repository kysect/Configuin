using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Tests.Resources;

public static class WellKnownRoslynRuleDefinitions
{
    public static RoslynStyleRule IDE0001()
    {
        return new RoslynStyleRule(
            ruleId: RoslynRuleId.Parse("IDE0001"),
            title: "Simplify name",
            category: "Style",
            overview: "This rule concerns the use of simplified type names in declarations and executable code, when possible. You can remove unnecessary name qualification to simplify code and improve readability.",
            example: """
                     using System.IO;
                     class C
                     {
                         // IDE0001: 'System.IO.FileInfo' can be simplified to 'FileInfo'
                         System.IO.FileInfo file;
                     
                         // Fixed code
                         FileInfo file;
                     }
                     """,
            options: Array.Empty<RoslynStyleRuleOption>());
    }


    public static RoslynStyleRule IDE0040()
    {
        string codeSample = """
                         // dotnet_style_require_accessibility_modifiers = always
                         // dotnet_style_require_accessibility_modifiers = for_non_interface_members
                         class MyClass
                         {
                             private const string thisFieldIsConst = "constant";
                         }

                         // dotnet_style_require_accessibility_modifiers = never
                         class MyClass
                         {
                             const string thisFieldIsConst = "constant";
                         }
                         """;

        RoslynStyleRuleOptionValue[] expectedOptionValues =
        {
            new RoslynStyleRuleOptionValue("always", "Prefer accessibility modifiers to be specified."),
            new RoslynStyleRuleOptionValue("for_non_interface_members", "Prefer accessibility modifiers except for public interface members."),
            new RoslynStyleRuleOptionValue("never", "Do not prefer accessibility modifiers to be specified."),
            new RoslynStyleRuleOptionValue("omit_if_default", "Prefer accessibility modifiers except if they are the default modifier.")
        };

        var expectedOption = new RoslynStyleRuleOption(
            Name: "dotnet_style_require_accessibility_modifiers",
            expectedOptionValues,
            DefaultValue: "for_non_interface_members",
            codeSample);

        return new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0040"),
            "Add accessibility modifiers",
            "Style",
            "This style rule concerns requiring accessibility modifiers in declarations.",
            null,
            new[] { expectedOption });
    }

    public static RoslynQualityRule CA1064()
    {
        return new RoslynQualityRule(
            ruleId: RoslynRuleId.Parse("CA1064"),
            title: "Exceptions should be public",
            category: "Design",
            description: """
                         An internal exception is only visible inside its own internal scope. After the exception falls outside the internal scope, only the base exception can be used to catch the exception. If the internal exception is inherited from xref:System.Exception, xref:System.SystemException, or xref:System.ApplicationException, the external code will not have sufficient information to know what to do with the exception.
                         But, if the code has a public exception that later is used as the base for an internal exception, it is reasonable to assume the code further out will be able to do something intelligent with the base exception. The public exception will have more information than what is provided by xref:System.Exception, xref:System.SystemException, or xref:System.ApplicationException.
                         """);
    }
}