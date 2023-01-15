using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal;

/// <summary>
///     Only used as pseudo doc when doing bulk updates/inserts.
/// </summary>
public class BulkDeleteDocuments : BulkDocuments
{
    public BulkDeleteDocuments(IEnumerable<ICouchDocument> docs) : base(docs)
    {
    }

    public override void WriteJson(JsonWriter writer)
    {
        writer.WritePropertyName("docs");
        writer.WriteStartArray();
        foreach (var doc in Docs)
        {
            writer.WriteStartObject();
            Document.WriteIdAndRev(doc, writer);
            writer.WritePropertyName("_deleted");
            writer.WriteValue(true);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }

    public override void ReadJson(JObject obj)
    {
        throw new NotImplementedException();
    }
}