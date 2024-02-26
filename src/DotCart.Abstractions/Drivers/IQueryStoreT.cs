using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Drivers;

public interface IQueryStoreT<in TQuery>
    : IStoreB
    where TQuery : IQueryB
{
    Task<IFeedback> ExecuteAsync(TQuery query, IFeedback previous = null);
}