namespace DotCart.Schema;


public delegate TID NewSimpleID<out TID>() where TID:SimpleID;


public record SimpleID 
{
    public string Prefix { get; set; }
    public string Value { get; set; }

    public string Id()
    {
        return $"{Prefix}-{Value}";
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