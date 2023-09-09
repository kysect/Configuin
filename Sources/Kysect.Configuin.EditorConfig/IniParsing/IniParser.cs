namespace Kysect.Configuin.EditorConfig.IniParsing;

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

            // TODO: #37 support case when comment is not in string start. Like:
            // key = value # some comment with symbol =
            if (line.StartsWith("#"))
                continue;

            // TODO: #38 support categories in future
            if (line.StartsWith("["))
                continue;

            if (!line.Contains('='))
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