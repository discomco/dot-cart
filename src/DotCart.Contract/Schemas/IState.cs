using DotCart.Contract.Dtos;

namespace DotCart.Contract.Schemas;

public delegate TState NewState<out TState>() where TState : IState;

public interface IState : IPayload
{
}