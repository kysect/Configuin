using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.DotnetConfig.Syntax.Nodes;
using System.Collections.Immutable;

namespace Kysect.Configuin.Tests.DotnetConfig.Tools;

public class DotnetConfigDocumentComparator
{
    public void Compare(DotnetConfigDocument actual, DotnetConfigDocument expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        CompareChildren(actual.Children, expected.Children);
        actual.TrailingTrivia.Should().BeEquivalentTo(expected.TrailingTrivia);
    }

    private void CompareChildren(ImmutableList<IDotnetConfigSyntaxNode> actual, ImmutableList<IDotnetConfigSyntaxNode> expected)
    {
        actual.Should().HaveCount(expected.Count);
        for (int i = 0; i < actual.Count; i++)
        {
            Compare(actual[i], expected[i]);
        }
    }

    private void CompareCategory(DotnetConfigCategoryNode actual, DotnetConfigCategoryNode expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        actual.Value.Should().Be(expected.Value);
        actual.LeadingTrivia.Should().BeEquivalentTo(expected.LeadingTrivia);
        actual.TrailingTrivia.Should().Be(expected.TrailingTrivia);
        CompareChildren(actual.Children, expected.Children);
    }

    private void CompareSection(DotnetConfigSectionNode actual, DotnetConfigSectionNode expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        actual.Value.Should().Be(expected.Value);
        actual.LeadingTrivia.Should().BeEquivalentTo(expected.LeadingTrivia);
        actual.TrailingTrivia.Should().Be(expected.TrailingTrivia);
        CompareChildren(actual.Children, expected.Children);
    }

    private void CompareProperty(IDotnetConfigPropertySyntaxNode actual, IDotnetConfigPropertySyntaxNode expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        actual.Value.Should().Be(expected.Value);
        actual.LeadingTrivia.Should().BeEquivalentTo(expected.LeadingTrivia);
        actual.TrailingTrivia.Should().Be(expected.TrailingTrivia);
        actual.Key.Should().Be(expected.Key);
        actual.Value.Should().Be(expected.Value);
    }

    private void Compare(IDotnetConfigSyntaxNode actual, IDotnetConfigSyntaxNode expected)
    {
        actual.GetType().Should().Be(expected.GetType());

        if (actual is DotnetConfigCategoryNode actualCategory)
        {
            CompareCategory(actualCategory, (DotnetConfigCategoryNode) expected);
            return;
        }

        if (actual is DotnetConfigSectionNode sectionNode)
        {
            CompareSection(sectionNode, (DotnetConfigSectionNode) expected);
            return;
        }

        if (actual is IDotnetConfigPropertySyntaxNode propertyNode)
        {
            CompareProperty(propertyNode, (IDotnetConfigPropertySyntaxNode) expected);
            return;
        }

        throw new NotSupportedException($"Cannot compare node of type {actual.GetType()}");
    }
}
