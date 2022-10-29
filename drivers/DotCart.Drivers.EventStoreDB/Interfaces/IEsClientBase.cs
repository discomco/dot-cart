namespace DotCart.Drivers.EventStoreDB.Interfaces;

public interface IEsClientBase : IDisposable, IAsyncDisposable
{
    string ConnectionName { get; }
}