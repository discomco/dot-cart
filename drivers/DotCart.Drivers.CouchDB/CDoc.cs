using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.CouchDB;

public record CDoc<T>
    where T : IState
{
    public T Data { get; set; }
    public DateTime TimeStamp { get; set; }
    public string _id { get; set; }
    public string _rev { get; set; }

    public static CDoc<TDoc> From<TDoc>(TDoc doc) where TDoc : IState
    {
        return new CDoc<TDoc>
        {
            _id = doc.Id,
            _rev = doc.Prev == "" ? null : doc.Prev,
            Data = doc,
            TimeStamp = DateTime.UtcNow
        };
    }
}