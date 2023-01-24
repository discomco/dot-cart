using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.CouchDB;

public record CDoc<T>
    where T : IState
{
    public T Data { get; set; }
    public DateTime TimeStamp { get; set; }
    public string _id { get; set; }
    public string? _rev { get; set; }
}

public static partial class Extensions
{
    public static CDoc<TDoc> AsCDoc<TDoc>(this TDoc doc) where TDoc : IState
    {
        return new CDoc<TDoc>
        {
            _id = doc.Id,
            _rev = doc.Rev == ""
                ? null
                : doc.Rev,
            Data = doc,
            TimeStamp = DateTime.UtcNow
        };
    }
}