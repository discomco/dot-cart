using DotCart.Abstractions.Schema;

namespace DotCart.Context.Actors;

public interface IProviderT<in TQuery> : IProviderB
    where TQuery : IQueryB
{
    Task<Feedback> GetDataAsync(TQuery query, Feedback previous=null);
}

public interface IProviderB
{
}