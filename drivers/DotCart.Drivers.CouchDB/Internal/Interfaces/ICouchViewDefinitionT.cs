using DotCart.Drivers.CouchDB.Internal.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal.Interfaces;

public interface ICouchViewDefinitionT : ICouchViewDefinitionB
{
    string Map { get; set; }
    string Reduce { get; set; }
    IEnumerable<T> All<T>() where T : ICouchDocument, new();
    bool Equals(ICouchViewDefinitionT other);
    IEnumerable<T> Key<T>(object key) where T : ICouchDocument, new();
    IEnumerable<T> KeyStartEnd<T>(object[] start, object[] end) where T : ICouchDocument, new();
    IEnumerable<T> KeyStartEnd<T>(object start, object end) where T : ICouchDocument, new();
    CouchLinqQuery<T> LinqQuery<T>();
    CouchQuery Query();
    void ReadJson(JObject obj);
    void Touch();
    void WriteJson(JsonWriter writer);
}