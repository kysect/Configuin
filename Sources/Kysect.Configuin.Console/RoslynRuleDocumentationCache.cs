using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.Configuin.Common;
using Kysect.Configuin.RoslynModels;
using System.Text.Json;

namespace Kysect.Configuin.Console;

public static class RoslynRuleDocumentationCache
{
    public static RoslynRules ReadFromCache()
    {
        var fileName = "roslyn-rules.json";
        if (!File.Exists(fileName))
        {
            throw new ConfiguinException($"File with {fileName} was not found");
        }

        string readAllText = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<RoslynRules>(readAllText).ThrowIfNull();
    }
}