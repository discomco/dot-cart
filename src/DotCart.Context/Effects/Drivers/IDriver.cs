namespace DotCart.Context.Effects.Drivers;

public interface IDriver : IDisposable
{
    void SetReactor(IReactor reactor);
}