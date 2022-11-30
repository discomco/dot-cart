using DotCart.Core;

namespace DotCart.Abstractions.Schema;

public interface IID
{
    string Value { get; }
    string Id();
}

public record ID : IID
{
    protected ID(string prefix, string value = "")
    {
        if (value == string.Empty)
            value = GuidUtils.LowerCaseGuid;
        Prefix = prefix.CheckPrefix();
        Value = value.CheckValue();
    }

    public static IID New(string prefix, string value = "")
    {
        return new ID(prefix, value);
    }
    

    public string Prefix { get; set; }
    public string Value { get; set; }

    public string Id()
    {
        return $"{Prefix}{IDFuncs.PrefixSeparator}{Value}";
    }
    
    
    
    
    
}

public static class IDFuncs
{
    public const char PrefixSeparator = '.';

    
    
    public static IID IDFromIdString(this string idString)
    {
        if (string.IsNullOrEmpty(idString) || !idString.Contains(PrefixSeparator))
            throw new ArgumentException($"idString must not be null or empty and must contain '{PrefixSeparator}' ");
        var parts = idString.Split(PrefixSeparator);
        return ID.New(parts[0], parts[1]);
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