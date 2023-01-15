using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal;

public class CouchRecord<T> where T : ICanJson, new()
{
    private readonly JObject _record;

    public CouchRecord(JObject source)
    {
        _record = source;

        Id = _record.Value<string>("id");
        Key = _record["key"];
        Value = _record["value"];
    }

    public string Id { get; }
    public JToken Key { get; }
    public JToken Value { get; }

    public T Document
    {
        get
        {
            if (!_record.TryGetValue("doc", out var val)) return default;

            var doc = val as JObject;
            if (doc == null) return default;

            var ret = new T();
            ret.ReadJson(doc);
            return ret;
        }
    }
}