namespace DotCart.Abstractions.Schema;

public delegate TState StateCtorT<out TState>()
    where TState : IState;