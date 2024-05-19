using System.Collections.Immutable;

namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public interface IDotnetConfigPropertySyntaxNode : IDotnetConfigSyntaxNode
{
    string Key { get; }
    string Value { get; }
    ImmutableList<string> LeadingTrivia { get; }
    string? TrailingTrivia { get; }

    IDotnetConfigPropertySyntaxNode WithLeadingTrivia(ImmutableList<string> leadingTrivia);
    IDotnetConfigPropertySyntaxNode WithTrailingTrivia(string? trailingTrivia);
}