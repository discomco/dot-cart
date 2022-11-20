namespace DotCart.Abstractions.Schema;

public delegate TState NewState<out TState>() where TState : IState;

public interface IState : IPayload
{
}