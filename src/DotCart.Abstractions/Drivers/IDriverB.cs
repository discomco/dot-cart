using DotCart.Abstractions.Actors;

namespace DotCart.Abstractions.Drivers;

public interface IDriverB : IDisposable
{
    void SetActor(IActor actor);
}