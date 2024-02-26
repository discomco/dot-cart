using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.Schema;

namespace DotCart.Actors;

public interface IProviderT<in TQuery> : IProviderB
    where TQuery : IQueryB
{
    Task<IFeedback> GetDataAsync(
        TQuery query,
        IFeedback previous = null);
}

public interface IProviderB
{
}