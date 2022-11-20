namespace DotCart.Abstractions.Behavior;

public interface IApply
{
    string EvtType { get; }
    void SetAggregate(IAggregate aggregate);
}

// public interface IApply<TState, in TEvt> : IApply
//     where TEvt : IEvt
//     where TState : IState
// {
//     TState Apply(TState state, Event evt);
// }