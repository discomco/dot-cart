namespace DotCart.Abstractions.Schema;

public interface IInvalidStateCtor {}
public interface IValidStateCtor {}

public delegate TState StateCtorT<out TState>() 
    where TState : IState;