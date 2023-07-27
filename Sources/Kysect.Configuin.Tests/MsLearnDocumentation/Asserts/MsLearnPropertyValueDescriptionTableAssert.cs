using FluentAssertions;
using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;

namespace Kysect.Configuin.Tests.MsLearnDocumentation.Asserts;

public class MsLearnPropertyValueDescriptionTableAssert
{
    private readonly MsLearnPropertyValueDescriptionTable _msLearnTableContent;

    public static MsLearnPropertyValueDescriptionTableAssert That(MsLearnPropertyValueDescriptionTable msLearnTableContent)
    {
        return new MsLearnPropertyValueDescriptionTableAssert(msLearnTableContent);
    }

    public MsLearnPropertyValueDescriptionTableAssert(MsLearnPropertyValueDescriptionTable msLearnTableContent)
    {
        _msLearnTableContent = msLearnTableContent;
    }

    public MsLearnPropertyValueDescriptionTableAssert RowCountIs(int count)
    {
        _msLearnTableContent.Properties.Should().HaveCount(count);

        return this;
    }

    public MsLearnPropertyValueDescriptionTableRowAssert HasProperty(string propertyName)
    {
        _msLearnTableContent.Properties.Should().ContainKey(propertyName);

        return new MsLearnPropertyValueDescriptionTableRowAssert(_msLearnTableContent.Properties[propertyName]);
    }
}