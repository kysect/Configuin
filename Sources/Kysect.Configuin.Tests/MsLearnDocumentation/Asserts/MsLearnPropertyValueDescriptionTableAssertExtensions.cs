using FluentAssertions;
using FluentAssertions.Collections;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using Kysect.Configuin.Core.RoslynRuleModels;

namespace Kysect.Configuin.Tests.MsLearnDocumentation.Asserts;

public static class MsLearnPropertyValueDescriptionTableAssertExtensions
{
    public static AndWhichConstraint<GenericCollectionAssertions<MsLearnPropertyValueDescriptionTableRow>, MsLearnPropertyValueDescriptionTableRow> Contain(
        this GenericCollectionAssertions<MsLearnPropertyValueDescriptionTableRow> assert,
        string value,
        string? description = null)
    {
        return assert.Contain(new MsLearnPropertyValueDescriptionTableRow(value, description ?? string.Empty));
    }

    public static AndWhichConstraint<GenericCollectionAssertions<RoslynStyleRule>, RoslynStyleRule> ContainRule(
        this GenericCollectionAssertions<RoslynStyleRule> assert,
        RoslynStyleRule value)
    {
        return assert.Contain(value);
    }
}