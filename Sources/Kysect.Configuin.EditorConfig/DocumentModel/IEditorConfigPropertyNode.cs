using System.Collections.Immutable;

namespace Kysect.Configuin.EditorConfig.DocumentModel;

public interface IEditorConfigPropertyNode : IEditorConfigNode
{
    string Key { get; }
    string Value { get; }
    ImmutableList<string> LeadingTrivia { get; }
    string? TrailingTrivia { get; }

    IEditorConfigPropertyNode WithLeadingTrivia(ImmutableList<string> leadingTrivia);
    IEditorConfigPropertyNode WithTrailingTrivia(string? trailingTrivia);
}