using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Defaults.Elastic;

public interface IElasticStoreT<TDoc> : IDocStoreT<TDoc>
    where TDoc : IState
{
}