using Kysect.Configuin.MsLearn.Tables.Models;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.MsLearn.Models;

public record RoslynStyleRuleInformationTable(
    RoslynRuleId RuleId,
    string Title,
    string Category,
    string Subcategory,
    string ApplicableLanguages,
    IReadOnlyCollection<string> Options)
{
    public static RoslynStyleRuleInformationTable Create(MsLearnPropertyValueDescriptionTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        MsLearnPropertyValueDescriptionTableRow ruleId = table.GetSingleValue("Rule ID");
        MsLearnPropertyValueDescriptionTableRow title = table.GetSingleValue("Title");
        MsLearnPropertyValueDescriptionTableRow category = table.GetSingleValue("Category");
        MsLearnPropertyValueDescriptionTableRow subcategory = table.GetSingleValue("Subcategory");
        MsLearnPropertyValueDescriptionTableRow applicableLanguages = table.GetSingleValue("Applicable languages");
        // TODO: return as optional parameter. Not all rules contains it
        //MsLearnPropertyValueDescriptionTableRow introducedVersion = table.GetSingleValue("Introduced version");
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