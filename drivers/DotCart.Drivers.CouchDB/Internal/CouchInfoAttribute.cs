using Ardalis.GuardClauses;
using DotCart.Drivers.Ardalis;

namespace DotCart.Drivers.CouchDB.Internal;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class CouchInfoAttribute : Attribute
{
    public CouchInfoAttribute(CouchInfoR couchInfo)
    {
        CouchInfo = couchInfo;
    }


    public CouchInfoR CouchInfo { get; set; }
}

public class CouchInfoR
{
    public string Host { get; set; }
}

public static class CouchInfoAtt
{
    public static CouchInfoR Get<T>()
    {
        var atts = (CouchInfoAttribute[])typeof(T).GetCustomAttributes(typeof(CouchInfoAttribute),
            true);
        Guard.Against.AttributeNotDefined("CouchInfo", atts, typeof(T).FullName);
        return atts[0].CouchInfo;
    }

    public static CouchInfoR Get(object obj)
    {
        var atts = (CouchInfoAttribute[])obj.GetType().GetCustomAttributes(typeof(CouchInfoAttribute),
            true);

        Guard.Against.AttributeNotDefined("CouchInfo", atts, obj.GetType().FullName);
        return atts[0].CouchInfo;
    }
}