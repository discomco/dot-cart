using DotCart.Schema;

namespace DotCart.Effects.Drivers;



public interface IModelStoreDriver<TState> : IProjectionDriver<TState>
    where TState : IState
{
}