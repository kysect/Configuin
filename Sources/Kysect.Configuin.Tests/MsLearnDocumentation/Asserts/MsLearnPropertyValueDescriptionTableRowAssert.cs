using FluentAssertions;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

namespace Kysect.Configuin.Tests.MsLearnDocumentation.Asserts;

public class MsLearnPropertyValueDescriptionTableRowAssert
{
    private readonly IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> _propertyValues;

    public MsLearnPropertyValueDescriptionTableRowAssert(IReadOnlyList<MsLearnPropertyValueDescriptionTableRow> propertyValues)
    {
        _propertyValues = propertyValues;
    }

    public MsLearnPropertyValueDescriptionTableRowAssert WithValue(string key, string? value = null)
    {
        _propertyValues.Should().Contain(new MsLearnPropertyValueDescriptionTableRow(key, value ?? string.Empty));

        return this;
    }
}