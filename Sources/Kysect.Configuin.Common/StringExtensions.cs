using System;

namespace Kysect.Configuin.Common;

public static class StringExtensions
{
    public static bool StartWithIgnoreCase(this string value, string otherValue)
    {
        return value.Substring(0, otherValue.Length).Equals(otherValue, StringComparison.InvariantCultureIgnoreCase);
    }

    public static string RemovePrefix(this string value, string prefix)
    {
        if (!value.StartWithIgnoreCase(prefix))
            throw new ArgumentException($"String {value} does not start with {prefix}");

        return value.Substring(prefix.Length, value.Length - prefix.Length);
    }
}