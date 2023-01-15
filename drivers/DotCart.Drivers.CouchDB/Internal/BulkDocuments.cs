using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal;

/// <summary>
///     Only used as psuedo doc when doing bulk updates/inserts.
/// </summary>
public class BulkDocuments : ICanJson
{
    public BulkDocuments(IEnumerable<ICouchDocument> docs)
    {
        Docs = docs;
    }

    public IEnumerable<ICouchDocument> Docs { get; }

    #region ICouchBulk Members

    public int Count()
    {
        return Docs.Count();
    }

    public virtual void WriteJson(JsonWriter writer)
    {
        writer.WritePropertyName("docs");
        writer.WriteStartArray();
        foreach (var doc in Docs)
            if (doc is ISelfContained)
            {
                doc.WriteJson(writer);
            }
            else
            {
                writer.WriteStartObject();
                doc.WriteJson(writer);
                writer.WriteEndObject();
            }

        writer.WriteEndArray();
    }

    public virtual void ReadJson(JObject obj)
    {
        throw new NotImplementedException();
    }

    #endregion
}