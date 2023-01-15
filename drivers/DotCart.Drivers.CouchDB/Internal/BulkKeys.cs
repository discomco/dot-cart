using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal;

/// <summary>
///     Only used as psuedo doc when doing bulk reads.
/// </summary>
public class BulkKeys : ICanJson
{
    public BulkKeys(IEnumerable<object> keys)
    {
        Keys = keys.ToArray();
    }

    public BulkKeys()
    {
    }

    public BulkKeys(object[] keys)
    {
        Keys = keys;
    }

    public object[] Keys { get; set; }

    #region ICouchBulk Members

    public virtual void WriteJson(JsonWriter writer)
    {
        writer.WritePropertyName("keys");
        writer.WriteStartArray();
        foreach (var id in Keys) writer.WriteValue(id);
        writer.WriteEndArray();
    }

    public virtual void ReadJson(JObject obj)
    {
        throw new NotImplementedException();
    }

    public int Count()
    {
        return Keys.Count();
    }

    #endregion
}