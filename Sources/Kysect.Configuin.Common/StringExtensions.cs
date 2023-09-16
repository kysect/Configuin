using Kysect.CommonLib.BaseTypes.Extensions;

namespace Kysect.Configuin.Common;

public static class StringExtensions
{
    public static string WithoutPrefix(this string value, string prefix)
    {
        value.ThrowIfNull();
        prefix.ThrowIfNull();

        if (!value.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException($"String {value} does not start with {prefix}");

        return value.Substring(prefix.Length, value.Length - prefix.Length);
    }
}