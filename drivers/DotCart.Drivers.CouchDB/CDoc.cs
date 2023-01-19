using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.CouchDB;

public class CDoc<T>  
    where T : IState
{
    public T Data { get; set; }
    public DateTime TimeStamp { get; set; }
    public string _id { get; set; }
    public string _rev { get; set; }
}