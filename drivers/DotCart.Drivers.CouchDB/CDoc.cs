using CouchDB.Driver.Types;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.CouchDB;

public class CDoc<T> : CouchDocument where T : IState
{
    public T Data { get; set; }
    public DateTime TimeStamp { get; set; }
}