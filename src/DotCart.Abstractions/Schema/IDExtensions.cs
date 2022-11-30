using System.Text.RegularExpressions;
using DotCart.Core;

namespace DotCart.Abstractions.Schema;

public static class IDExtensions
{
    public const string DefaultPrefix = "id";

    public static readonly Regex PrefixRegex = new(@"^[a-z]*$", RegexOptions.Compiled);

    private static readonly Regex ValueRegex = new(
        @"^(?<guid>[a-f0-9]{8}\-[a-f0-9]{4}\-[a-f0-9]{4}\-[a-f0-9]{4}\-[a-f0-9]{12})$",
        RegexOptions.Compiled);

    public static string CheckPrefix(this string prefix)
    {
        if (string.IsNullOrEmpty(prefix)) return DefaultPrefix;
        var match = PrefixRegex.IsMatch(prefix);
        return !match ? "invalid" : prefix;
    }

    public static string CheckValue(this string value)
    {
        if (string.IsNullOrEmpty(value))
            value = GuidUtils.LowerCaseGuid;
        value = value.ToLower();
        var match = ValueRegex.IsMatch(value);
        if (!match) throw new Exception($"{value} cannot be used as an identifier for an ID. Must be UUID string");
        return value;
    }
}