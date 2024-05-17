using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.MsLearn.Models;

namespace Kysect.Configuin.MsLearn;

public class MsLearnDocumentationPreprocessor
{
    public MsLearnDocumentationRawInfo Process(MsLearnDocumentationRawInfo info)
    {
        return new MsLearnDocumentationRawInfo(
            info.QualityRuleFileContents.Select(Process).ToList(),
            info.StyleRuleFileContents.Select(Process).ToList(),
            Process(info.SharpFormattingOptionsContent),
            Process(info.DotnetFormattingOptionsContent));
    }

    public string Process(string input)
    {
        input.ThrowIfNull();

        // TODO: remove this hack
        input = input
            .Replace("\r\n", "\n")
            .Replace("\n", Environment.NewLine);

        List<string> lines = input.Split(Environment.NewLine).ToList();

        lines = RemoveZones(lines);

        return string.Join(Environment.NewLine, lines);
    }

    private List<string> RemoveZones(List<string> lines)
    {
        // TODO: Performance is not so good
        while (lines.Any(l => l.StartsWith(":::zone ")))
        {
            int startIndex = lines.FindIndex(l => l.StartsWith(":::zone "));
            int endIndex = lines.FindIndex(l => l.StartsWith(":::zone-end"));

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
                throw new ArgumentException("Cannot find zones for removing");

            if (lines[startIndex].StartsWith(":::zone pivot=\"lang-csharp-vb\""))
            {
                lines.RemoveAt(endIndex);
                lines.RemoveAt(startIndex);
                continue;
            }

            if (lines[startIndex].StartsWith(":::zone pivot=\"lang-fsharp\""))
            {
                lines.RemoveRange(startIndex, endIndex - startIndex + 1);
                continue;
            }

            throw new ArgumentException($"Unsupported zone {lines[startIndex]}");
        }

        return lines;
    }
}