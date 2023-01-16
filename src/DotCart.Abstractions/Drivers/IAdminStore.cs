namespace DotCart.Abstractions.Drivers;

public interface IAdminStore<TDbInfo>
    : IClose, IDisposable, IAsyncDisposable, ICloseAsync
    where TDbInfo : IDbInfoB
{
}