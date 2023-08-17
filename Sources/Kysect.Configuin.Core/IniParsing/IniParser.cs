namespace Kysect.Configuin.Core.IniParsing;

public class IniParser
{
    public IReadOnlyCollection<IniFileLine> Parse(string content)
    {
        string[] lines = content.Split(Environment.NewLine);

        var result = new List<IniFileLine>();

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            // TODO: support case when comment is not in string start. Like:
            // key = value # some comment with symbol =
            if (line.StartsWith("#"))
                continue;

            // TODO: support categories in future
            if (line.StartsWith("["))
                continue;

            // TODO: remove rule that force StringComparison for string comparing from project .editorconfig
            if (!line.Contains('=', StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException($"Line {line} does not contain '='");

            string[] parts = line.Split('=');
            if (parts.Length != 2)
                throw new ArgumentException($"Line {line} contains unexpected count of '='");

            string key = parts[0].Trim();
            string value = parts[1].Trim();
            result.Add(new IniFileLine(key, value));
        }

        return result;
    }
}