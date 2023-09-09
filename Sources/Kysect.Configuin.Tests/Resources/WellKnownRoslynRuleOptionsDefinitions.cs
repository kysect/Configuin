using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests.Resources;

public static class WellKnownRoslynRuleOptionsDefinitions
{
    public static RoslynStyleRuleOption dotnet_style_qualification_for_field()
    {
        // TODO: add dots to end of lines into source code and fix test
        return new RoslynStyleRuleOption(
            "dotnet_style_qualification_for_field",
            new[]
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
            """);
    }

    public static RoslynStyleRuleOption dotnet_style_qualification_for_property()
    {
        return new RoslynStyleRuleOption(
            "dotnet_style_qualification_for_property",
            new[]
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
            """);
    }

    public static RoslynStyleRuleOption dotnet_style_qualification_for_method()
    {
        return new RoslynStyleRuleOption(
            "dotnet_style_qualification_for_method",
            new[]
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
            """);
    }

    public static RoslynStyleRuleOption dotnet_style_qualification_for_event()
    {
        return new RoslynStyleRuleOption(
            "dotnet_style_qualification_for_event",
            new[]
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
            """);
    }

    public static RoslynStyleRuleOption csharp_new_line_before_open_brace()
    {
        return new RoslynStyleRuleOption(
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
    }

    public static RoslynStyleRuleOption dotnet_sort_system_directives_first()
    {
        return new RoslynStyleRuleOption(
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
    }

    public static RoslynStyleRuleOption dotnet_separate_import_directive_groups()
    {
        return new RoslynStyleRuleOption(
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
    }

    public static RoslynStyleRuleOption dotnet_style_operator_placement_when_wrapping()
    {
        return new RoslynStyleRuleOption(
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
    }
}