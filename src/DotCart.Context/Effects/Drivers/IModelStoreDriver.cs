using DotCart.Context.Schemas;

namespace DotCart.Context.Effects.Drivers;

public interface IModelStoreDriver<TState> : IProjectionDriver<TState>
    where TState : IState
{
}