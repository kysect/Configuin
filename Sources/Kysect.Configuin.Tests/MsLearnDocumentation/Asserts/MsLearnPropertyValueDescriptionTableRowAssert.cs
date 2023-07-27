using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using NUnit.Framework;

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
        Assert.That(_propertyValues, Does.Contain(new MsLearnPropertyValueDescriptionTableRow(key, value ?? string.Empty)));
        return this;
    }
}