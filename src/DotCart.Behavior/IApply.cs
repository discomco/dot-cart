namespace DotCart.Behavior;

public interface IApply {}

public interface IApply<in TEvt>: IApply
    where TEvt: IEvt
{
    void Apply(TEvt evt);
}