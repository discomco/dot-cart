using DotCart.Core;

namespace DotCart.Contract.Schemas;

public delegate TID NewID<out TID>() where TID : IID;

public interface IID
{
    string Value { get; }
    string Id();
}

public record ID : IID
{
    public ID(string prefix, string value = "")
    {
        if (value == string.Empty)
            value = GuidUtils.LowerCaseGuid;
        Prefix = prefix.CheckPrefix();
        Value = value.CheckValue();
    }

    public string Prefix { get; set; }
    public string Value { get; set; }

    public string Id()
    {
        return $"{Prefix}{IDFuncs.PrefixSeparator}{Value}";
    }

    public static ID New(string prefix)
    {
        return new ID(prefix);
    }
}

public static class IDFuncs
{
    public const char PrefixSeparator = '.';

    public static ID IDFromIdString(this string idString)
    {
        if (string.IsNullOrEmpty(idString) || !idString.Contains(PrefixSeparator))
            throw new ArgumentException($"idString must not be null or empty and must contain '{PrefixSeparator}' ");
        var parts = idString.Split(PrefixSeparator);
        return new ID(parts[0], parts[1]);
    }
}