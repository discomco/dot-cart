using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface ISequenceT<TPayload>
    where TPayload : IPayload
{
    Task<Feedback> ExecuteAsync(IDto msg, CancellationToken cancellationToken = default);
    string Name { get; }
    int StepCount { get; }
}