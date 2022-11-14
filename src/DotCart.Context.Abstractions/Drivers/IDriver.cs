namespace DotCart.Context.Abstractions.Drivers;

public interface IDriver : IDisposable
{
    void SetActor(IActor actor);
}