using System.Text;
using ArgumentNullException = System.ArgumentNullException;

namespace Kysect.Configuin.EditorConfig.Template;

public class EditorConfigTemplateBuilder
{
    private readonly StringBuilder _templateBuilder = new StringBuilder();

    public void AddCommentString(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        foreach (string s in FormatString(value))
            _templateBuilder.AppendLine($"# {s}");
    }

    public void AddDoubleCommentString(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        foreach (string s in FormatString(value))
            _templateBuilder.AppendLine($"## {s}");
    }

    public void AddEmptyLine()
    {
        _templateBuilder.AppendLine();
    }

    public string Build()
    {
        return _templateBuilder.ToString();
    }

    private string[] FormatString(string value)
    {
        return value.Split(Environment.NewLine);
    }
}