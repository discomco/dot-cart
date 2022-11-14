using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions;

public interface ICmdHandler
{
    Task<IFeedback> HandleAsync(ICmd cmd, CancellationToken cancellationToken = default);
}