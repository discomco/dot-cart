namespace DotCart.Schema;

public delegate TState NewState<out TState>() where TState:IState;


public interface IState
{ }