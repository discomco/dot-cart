using DotCart.Schema;

namespace DotCart.Effects.Drivers;

public interface IProjectionDriver<TState> : IModelStoreDriver<TState>
    where TState : IState
{
}