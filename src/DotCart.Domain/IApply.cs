using DotCart.Schema;

namespace DotCart.Domain;

public interface IApply {}

public interface IApply<TState, in TEvt>: IApply
    where TState: IState 
    where TEvt: IEvt
{
    TState Apply(TState state, TEvt evt);
}