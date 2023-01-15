using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal;

/// <summary>
///     A CouchDocument that holds its contents as a parsed JObject DOM which can be used
///     as a "light weight" base document instead of CouchDocument.
///     The _id and _rev are held inside the JObject.
/// </summary>
public class JsonDocument : ICouchDocument
{
    public JsonDocument(string json, string id, string rev)
    {
        Obj = JObject.Parse(json);
        Id = id;
        Rev = rev;
    }

    public JsonDocument(string json, string id)
    {
        Obj = JObject.Parse(json);
        Id = id;
    }

    public JsonDocument(string json)
    {
        Obj = JObject.Parse(json);
    }

    public JsonDocument(JObject doc)
    {
        Obj = doc;
    }

    public JsonDocument()
    {
        Obj = new JObject();
    }

    public JObject Obj { get; set; }

    public override string ToString()
    {
        return Obj.ToString();
    }

    #region ICouchDocument Members

    public virtual void WriteJson(JsonWriter writer)
    {
        foreach (var token in Obj.Children()) token.WriteTo(writer);
    }

    // Presume that Obj has _id and _rev
    public void ReadJson(JObject obj)
    {
        Obj = obj;
    }

    public string Rev
    {
        get
        {
            if (Obj["_rev"] == null) return null;
            return Obj["_rev"].Value<string>();
        }
        set => Obj["_rev"] = JToken.FromObject(value);
    }

    public string Id
    {
        get
        {
            if (Obj["_id"] == null) return null;
            return Obj["_id"].Value<string>();
        }
        set => Obj["_id"] = JToken.FromObject(value);
    }

    #endregion
}