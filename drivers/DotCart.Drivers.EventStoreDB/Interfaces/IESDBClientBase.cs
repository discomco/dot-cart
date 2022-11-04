namespace DotCart.Drivers.EventStoreDB.Interfaces;

public interface IESDBClientBase : IDisposable, IAsyncDisposable
{
    string ConnectionName { get; }
}