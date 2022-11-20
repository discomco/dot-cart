using DotCart.Abstractions.Schema;

namespace DotCart.Context.Schemas;

public delegate TStateRepo NewStateRepo<out TStateRepo, TState>()
    where TStateRepo : IStateRepo<TState>
    where TState : IState;

public interface IStateRepo<TState> where TState : IState
{
}