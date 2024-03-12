namespace DotCart.Drivers.EventStoreDB;

public interface IESDBClientBase
    : IDisposable, IAsyncDisposable
{
    string ConnectionName { get; }
}