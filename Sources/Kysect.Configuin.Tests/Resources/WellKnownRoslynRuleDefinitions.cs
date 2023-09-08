﻿using Kysect.Configuin.Core.RoslynRuleModels;

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
            RoslynRuleId.Parse("CA1064"),
            "Exceptions should be public",
            "Design",
            // TODO: parse description
            string.Empty);
    }
}