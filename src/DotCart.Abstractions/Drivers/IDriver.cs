using DotCart.Abstractions.Actors;

namespace DotCart.Abstractions.Drivers;

public interface IDriver : IDisposable
{
    void SetActor(IActor actor);
}