using FluentAssertions;
using Kysect.Configuin.RoslynModels;
using NUnit.Framework;

namespace Kysect.Configuin.Tests;

public class RoslynRuleIdTests
{
    [Test]
    [TestCase("CA1000", RoslynRuleType.QualityRule, 1000)]
    [TestCase("ca1001", RoslynRuleType.QualityRule, 1001)]
    [TestCase("IDE1002", RoslynRuleType.StyleRule, 1002)]
    public void Parse(string input, RoslynRuleType type, int id)
    {
        var expected = new RoslynRuleId(type, id);

        var roslynRuleId = RoslynRuleId.Parse(input);

        roslynRuleId.Should().Be(expected);
    }

    [Test]
    public void Parse_WithIncorrectPrefix_ThrowException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var roslynRuleId = RoslynRuleId.Parse("QWE1234");
        });
    }

    [Test]
    public void Parse_Range_ReturnAllValueInRange()
    {
        string input = "CA1865-CA1867";

        var range = RoslynRuleIdRange.Parse(input);
        var roslynRuleIds = range.Enumerate().ToList();

        roslynRuleIds
            .Should().HaveCount(3)
            .And.Contain(new RoslynRuleId(RoslynRuleType.QualityRule, 1865))
            .And.Contain(new RoslynRuleId(RoslynRuleType.QualityRule, 1866))
            .And.Contain(new RoslynRuleId(RoslynRuleType.QualityRule, 1867));
    }
}