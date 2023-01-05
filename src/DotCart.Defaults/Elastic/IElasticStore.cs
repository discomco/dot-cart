using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Defaults.Elastic;

public interface IElasticStore<TDoc> : IDocStore<TDoc>
    where TDoc : IState
{
}