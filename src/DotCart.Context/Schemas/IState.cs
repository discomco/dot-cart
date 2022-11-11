using DotCart.Client.Contracts;

namespace DotCart.Context.Schemas;

public delegate TState NewState<out TState>() where TState : IState;

public delegate TStateRepo NewStateRepo<out TStateRepo, TState>()
    where TStateRepo : IStateRepo<TState>
    where TState : IState;

public interface IStateRepo<TState> where TState : IState
{
}

public interface IState : IPayload
{
}