using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ICmdHandler
{
    Task<Feedback> HandleAsync(ICmdB cmd, CancellationToken cancellationToken = default);
}