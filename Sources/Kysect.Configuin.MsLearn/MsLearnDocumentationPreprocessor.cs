using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.MsLearn.Models;

namespace Kysect.Configuin.MsLearn;

public class MsLearnDocumentationPreprocessor
{
    public MsLearnDocumentationRawInfo Process(MsLearnDocumentationRawInfo info)
    {
        info.ThrowIfNull();

        return new MsLearnDocumentationRawInfo(
            info.QualityRuleFileContents.Select(Process).ToList(),
            info.StyleRuleFileContents.Select(Process).ToList(),
            Process(info.SharpFormattingOptionsContent),
            Process(info.DotnetFormattingOptionsContent),
            Process(info.QualityRuleOptions));
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

        lines = lines.Where(l => !l.StartsWith("[!INCLUDE")).ToList();

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

            bool actualZone = lines[startIndex].StartsWith(":::zone pivot=\"lang-csharp-vb\"")
                              || lines[startIndex].StartsWith(":::zone pivot=\"dotnet-8-0\"");
            if (actualZone)
            {
                lines.RemoveAt(endIndex);
                lines.RemoveAt(startIndex);
                continue;
            }

            bool notActualZone = lines[startIndex].StartsWith(":::zone pivot=\"lang-fsharp\"")
                                 || lines[startIndex].StartsWith(":::zone pivot=\"dotnet-7-0,dotnet-6-0\"");
            if (notActualZone)
            {
                lines.RemoveRange(startIndex, endIndex - startIndex + 1);
                continue;
            }

            throw new ArgumentException($"Unsupported zone {lines[startIndex]}");
        }

        while (lines.Any(l => l.StartsWith(":::row:::")))
        {
            int startIndex = lines.FindIndex(l => l.StartsWith(":::row:::"));
            int endIndex = lines.FindIndex(l => l.StartsWith(":::row-end:::"));

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
                throw new ArgumentException("Cannot find zones for removing");

            lines.RemoveRange(startIndex, endIndex - startIndex + 1);
        }

        return lines;
    }


}