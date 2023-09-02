using System;

namespace Kysect.Configuin.Common;

public static class StringExtensions
{
    public static string WithoutPrefix(this string value, string prefix)
    {
        if (!value.StartsWith(prefix))
            throw new ArgumentException($"String {value} does not start with {prefix}");

        return value.Substring(prefix.Length, value.Length - prefix.Length);
    }
}