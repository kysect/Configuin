namespace Kysect.Configuin.Core.CodeStyleGeneration.Models;

public class CodeStyleInfo
{
    public IReadOnlyCollection<ICodeStyleElement> Elements { get; }

    public CodeStyleInfo(IReadOnlyCollection<ICodeStyleElement> elements)
    {
        Elements = elements;
    }
}