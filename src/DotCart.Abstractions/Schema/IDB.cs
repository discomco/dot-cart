using DotCart.Core;

namespace DotCart.Abstractions.Schema;

public interface IID
{
    string Value { get; }
    string Id();
}

public record IDB : IID
{
    public IDB()
    {
    }


    protected IDB(string prefix, string value = "")
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

    public static IID New(string prefix, string value = "")
    {
        return new IDB(prefix, value);
    }
}

public static class IDFuncs
{
    public const char PrefixSeparator = '.';


    public static TID IDFromIdString<TID>(this string idString, IDCtorT<TID> ctor)
        where TID : IID
    {
        return ctor(idString.ValueFromIdString());
    }

    public static IID IDFromIdString(this string idString)
    {
        if (string.IsNullOrEmpty(idString) || !idString.Contains(PrefixSeparator))
            throw new ArgumentException($"idString must not be null or empty and must contain '{PrefixSeparator}' ");
        var parts = idString.Split(PrefixSeparator);
        return IDB.New(parts[0], parts[1]);
    }

    public static string PrefixFromIdString(this string idString)
    {
        var parts = idString.Split(PrefixSeparator);
        return parts[0];
    }

    public static string ValueFromIdString(this string idString)
    {
        var parts = idString.Split(PrefixSeparator);
        return parts[1];
    }
}