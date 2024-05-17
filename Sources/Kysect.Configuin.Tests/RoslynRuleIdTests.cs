using Kysect.Configuin.Common;
using Kysect.Configuin.RoslynModels;

namespace Kysect.Configuin.Tests;

public class RoslynRuleIdTests
{
    [Theory]
    [InlineData("CA1000", RoslynRuleTypes.QualityRule, 1000)]
    [InlineData("ca1001", RoslynRuleTypes.QualityRule, 1001)]
    [InlineData("IDE1002", RoslynRuleTypes.StyleRule, 1002)]
    [InlineData("CS0219", "CS", 0219)]
    public void Parse(string input, string type, int id)
    {
        var expected = new RoslynRuleId(type, id);

        var roslynRuleId = RoslynRuleId.Parse(input);

        roslynRuleId.Should().Be(expected);
    }

    [Fact]
    public void Parse_WithIncorrectPrefix_ThrowException()
    {
        Assert.Throws<ConfiguinException>(() =>
        {
            var roslynRuleId = RoslynRuleId.Parse("QWE234");
        });
    }

    [Fact]
    public void Parse_Range_ReturnAllValueInRange()
    {
        string input = "CA1865-CA1867";

        var range = RoslynRuleIdRange.Parse(input);
        var roslynRuleIds = range.Enumerate().ToList();

        roslynRuleIds
            .Should().HaveCount(3)
            .And.Contain(new RoslynRuleId("CA", 1865))
            .And.Contain(new RoslynRuleId("CA", 1866))
            .And.Contain(new RoslynRuleId("CA", 1867));
    }
}
