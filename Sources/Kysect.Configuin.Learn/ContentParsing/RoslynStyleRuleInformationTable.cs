using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Learn.ContentParsing;

public record RoslynStyleRuleInformationTable(
    RoslynRuleId RuleId,
    string Title,
    string Category,
    string Subcategory,
    string ApplicableLanguages,
    IReadOnlyCollection<string> Options)
{
    public static RoslynStyleRuleInformationTable Create(LearnPropertyValueDescriptionTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        LearnPropertyValueDescriptionTableRow ruleId = table.GetSingleValue("Rule ID");
        LearnPropertyValueDescriptionTableRow title = table.GetSingleValue("Title");
        LearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        LearnPropertyValueDescriptionTableRow subcategory = table.GetSingleValue("Subcategory");
        LearnPropertyValueDescriptionTableRow applicableLanguages = table.GetSingleValue("Applicable languages");
        // TODO: return as optional parameter. Not all rules contains it
        //LearnPropertyValueDescriptionTableRow introducedVersion = table.GetSingleValue("Introduced version");
        var options = table
            .FindValues("Options")
            .Select(o => o.Value)
            .ToList();

        return new RoslynStyleRuleInformationTable(
            RoslynRuleId.Parse(ruleId.Value),
            title.Value,
            category.Value,
            subcategory.Value,
            applicableLanguages.Value,
            options);
    }
}