namespace DotCart.Abstractions.Schema;

public delegate TState StateCtor<out TState>() where TState : IState;

public interface IState : IPayload
{
}