using DotCart.Schema;

namespace DotCart.Domain;

public interface IApply {}

public interface IApply<in TEvt>: IApply
    where TEvt: IEvt
{
    void Apply(TEvt evt);
}