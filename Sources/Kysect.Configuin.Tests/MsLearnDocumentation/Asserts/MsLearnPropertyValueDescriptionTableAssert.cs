using Kysect.Configuin.Core.MsLearnDocumentation.Tables.Models;
using NUnit.Framework;

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
        Assert.That(_msLearnTableContent.Properties.Count, Is.EqualTo(count));
        return this;
    }

    public MsLearnPropertyValueDescriptionTableRowAssert HasProperty(string propertyName)
    {
        Assert.True(_msLearnTableContent.Properties.ContainsKey(propertyName));
        return new MsLearnPropertyValueDescriptionTableRowAssert(_msLearnTableContent.Properties[propertyName]);
    }
}