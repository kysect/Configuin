using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.EditorConfig.DocumentModel;
using Kysect.Configuin.EditorConfig.DocumentModel.Nodes;
using System.Collections.Immutable;

namespace Kysect.Configuin.Tests.EditorConfig.Tools;

public class EditorConfigDocumentComparator
{
    public void Compare(EditorConfigDocument actual, EditorConfigDocument expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        CompareChildren(actual.Children, expected.Children);
        actual.TrailingTrivia.Should().BeEquivalentTo(expected.TrailingTrivia);
    }

    private void CompareChildren(ImmutableList<IEditorConfigNode> actual, ImmutableList<IEditorConfigNode> expected)
    {
        actual.Should().HaveCount(expected.Count);
        for (int i = 0; i < actual.Count; i++)
        {
            Compare(actual[i], expected[i]);
        }
    }

    private void CompareCategory(EditorConfigCategoryNode actual, EditorConfigCategoryNode expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        actual.Value.Should().Be(expected.Value);
        actual.LeadingTrivia.Should().BeEquivalentTo(expected.LeadingTrivia);
        actual.TrailingTrivia.Should().Be(expected.TrailingTrivia);
        CompareChildren(actual.Children, expected.Children);
    }

    private void CompareSection(EditorConfigDocumentSectionNode actual, EditorConfigDocumentSectionNode expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        actual.Value.Should().Be(expected.Value);
        actual.LeadingTrivia.Should().BeEquivalentTo(expected.LeadingTrivia);
        actual.TrailingTrivia.Should().Be(expected.TrailingTrivia);
        CompareChildren(actual.Children, expected.Children);
    }

    private void CompareProperty(EditorConfigPropertyNode actual, EditorConfigPropertyNode expected)
    {
        actual.ThrowIfNull();
        expected.ThrowIfNull();

        actual.Value.Should().Be(expected.Value);
        actual.LeadingTrivia.Should().BeEquivalentTo(expected.LeadingTrivia);
        actual.TrailingTrivia.Should().Be(expected.TrailingTrivia);
        actual.Key.Should().Be(expected.Key);
        actual.Value.Should().Be(expected.Value);
    }

    private void Compare(IEditorConfigNode actual, IEditorConfigNode expected)
    {
        actual.GetType().Should().Be(expected.GetType());

        if (actual is EditorConfigCategoryNode actualCategory)
        {
            CompareCategory(actualCategory, (EditorConfigCategoryNode) expected);
            return;
        }

        if (actual is EditorConfigDocumentSectionNode sectionNode)
        {
            CompareSection(sectionNode, (EditorConfigDocumentSectionNode) expected);
            return;
        }

        if (actual is EditorConfigPropertyNode propertyNode)
        {
            CompareProperty(propertyNode, (EditorConfigPropertyNode) expected);
            return;
        }

        throw new NotSupportedException($"Cannot compare node of type {actual.GetType()}");
    }
}
