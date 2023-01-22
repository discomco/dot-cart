using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IQueryStoreT<in TQuery>
    : IStoreB
    where TQuery : IQueryB
{
    Task<Feedback> ExecuteAsync(TQuery query, Feedback previous = null);
}