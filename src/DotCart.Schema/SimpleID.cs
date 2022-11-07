using System.Security.Cryptography.X509Certificates;

namespace DotCart.Schema;


public delegate TID NewSimpleID<out TID>() where TID:IID;


public interface IID
{
    string Value { get; }
    string Id();
}



public record SimpleID: IID 
{
    
    public string Prefix { get; set; }
    public string Value { get; set; }

    public string Id()
    {
        return $"{Prefix}{SimpleIDFuncs.PrefixSeparator}{Value}";
    }
    public SimpleID(string prefix, string value = "")
    {
        if (value == string.Empty) 
            value = GuidUtils.LowerCaseGuid;
        Prefix = prefix.CheckPrefix();
        Value = value.CheckValue();
    }
    
    public static SimpleID New(string prefix) => new(prefix);
    
}

public static class SimpleIDFuncs
{
    public const char PrefixSeparator = '=';
    public static SimpleID IDFromIdString(this string idString)
    {
        if (string.IsNullOrEmpty(idString) || !idString.Contains(PrefixSeparator))
        {
            throw new ArgumentException($"idString must not be null or empty and must contain '{PrefixSeparator}' ");
        }
        var parts = idString.Split(PrefixSeparator);
            return new SimpleID(parts[0], parts[1]);
    }
}


 