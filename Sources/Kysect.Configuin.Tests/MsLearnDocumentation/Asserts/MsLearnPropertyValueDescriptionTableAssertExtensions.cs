using FluentAssertions;
using FluentAssertions.Collections;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

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
}