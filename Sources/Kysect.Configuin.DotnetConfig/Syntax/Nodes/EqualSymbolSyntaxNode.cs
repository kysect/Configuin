namespace Kysect.Configuin.DotnetConfig.Syntax.Nodes;

public record EqualSymbolSyntaxNode(string LeadingTrivia, string TrailingTrivia)
{
    public static EqualSymbolSyntaxNode Empty { get; } = new EqualSymbolSyntaxNode(string.Empty, string.Empty);

    public string ToFullString()
    {
        return $"{LeadingTrivia}={TrailingTrivia}";
    }
}