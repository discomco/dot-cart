using DotCart.Contract.Dtos;

namespace DotCart.Context.Behaviors;

public interface ICmdHandler
{
    Task<IFeedback> HandleAsync(ICmd cmd, CancellationToken cancellationToken = default);
}
