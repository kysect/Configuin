﻿namespace Kysect.Configuin.Core.IniParsing;

public class IniParser
{
    public IReadOnlyCollection<InitFileLine> Parse(string content)
    {
        string[] lines = content.Split(Environment.NewLine);

        var result = new List<InitFileLine>();

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            if (line.StartsWith("#"))
                continue;

            // TODO: support categories in future
            if (line.StartsWith("["))
                continue;

            // TODO: remove rule that force StringComparison for string comparing from project .editorconfig
            if (!line.Contains('=', StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException($"Line {line} does not contains '='");

            string[] parts = line.Split('=');
            if (parts.Length != 2)
                throw new ArgumentException($"Line {line} contains unexpected count of '='");

            string key = parts[0].Trim();
            string value = parts[1].Trim();
            result.Add(new InitFileLine(key, value));
        }

        return result;
    }
}