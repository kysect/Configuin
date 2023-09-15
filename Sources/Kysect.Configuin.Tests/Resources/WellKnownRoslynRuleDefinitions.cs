using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests.Resources;

public static class WellKnownRoslynRuleDefinitions
{
    public static RoslynStyleRuleGroup IDE0001()
    {
        var rule = new RoslynStyleRule(
            RuleId: RoslynRuleId.Parse("IDE0001"),
            Title: "Simplify name",
            Category: "Style",
            Overview: "This rule concerns the use of simplified type names in declarations and executable code, when possible. You can remove unnecessary name qualification to simplify code and improve readability.",
            Example: """
                     using System.IO;
                     class C
                     {
                         // IDE0001: 'System.IO.FileInfo' can be simplified to 'FileInfo'
                         System.IO.FileInfo file;
                     
                         // Fixed code
                         FileInfo file;
                     }
                     """);

        return new RoslynStyleRuleGroup(rule);
    }

    public static RoslynStyleRuleGroup Ide0003_0009()
    {
        return new RoslynStyleRuleGroup(new[] { Ide0003(), Ide0009() }, Ide0003_0009Options());
    }

    public static RoslynStyleRule Ide0003()
    {
        var ide0003 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0003"),
            "Remove this or Me qualification",
            "Style",
            """
            These two rules define whether or not you prefer the use of this (C#) and Me. (Visual Basic) qualifiers. To enforce that the qualifiers aren't present, set the severity of IDE0003 to warning or error. To enforce that the qualifiers are present, set the severity of IDE0009 to warning or error.
            For example, if you prefer qualifiers for fields and properties but not for methods or events, then you can enable IDE0009 and set the options dotnet_style_qualification_for_field and dotnet_style_qualification_for_property to true. However, this configuration would not flag methods and events that do have this and Me qualifiers. To also enforce that methods and events don't have qualifiers, enable IDE0003.
            """,
            Example: null);
        return ide0003;
    }

    public static RoslynStyleRule Ide0009()
    {
        var ide0009 = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0009"),
            "Add this or Me qualification",
            "Style",
            """
            These two rules define whether or not you prefer the use of this (C#) and Me. (Visual Basic) qualifiers. To enforce that the qualifiers aren't present, set the severity of IDE0003 to warning or error. To enforce that the qualifiers are present, set the severity of IDE0009 to warning or error.
            For example, if you prefer qualifiers for fields and properties but not for methods or events, then you can enable IDE0009 and set the options dotnet_style_qualification_for_field and dotnet_style_qualification_for_property to true. However, this configuration would not flag methods and events that do have this and Me qualifiers. To also enforce that methods and events don't have qualifiers, enable IDE0003.
            """,
            null);
        return ide0009;
    }

    public static RoslynStyleRuleGroup IDE0040()
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

        var styleRule = new RoslynStyleRule(
            RoslynRuleId.Parse("IDE0040"),
            "Add accessibility modifiers",
            "Style",
            "This style rule concerns requiring accessibility modifiers in declarations.",
            Example: null);

        return new RoslynStyleRuleGroup(styleRule, new[] { expectedOption });
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

    public static RoslynQualityRule CA1865()
    {
        return new RoslynQualityRule(
            ruleId: RoslynRuleId.Parse("CA1865"),
            title: "Use 'string.Method(char)' instead of 'string.Method(string)' for string with single char",
            category: "Performance",
            description: "The overload that takes a char parameter performs better than the overload that takes a string parameter.");
    }

    public static RoslynStyleRuleOption[] Ide0003_0009Options()
    {
        var options = new RoslynStyleRuleOption[]
        {
            WellKnownRoslynRuleOptionsDefinitions.dotnet_style_qualification_for_field(),
            WellKnownRoslynRuleOptionsDefinitions.dotnet_style_qualification_for_property(),
            WellKnownRoslynRuleOptionsDefinitions.dotnet_style_qualification_for_method(),
            WellKnownRoslynRuleOptionsDefinitions.dotnet_style_qualification_for_event()
        };
        return options;
    }
}