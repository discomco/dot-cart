using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Abstractions.Actors;

public interface IStepT<TPayload>
    where TPayload : IPayload
{
    ISequenceT<TPayload> Sequence { get; }
    string Name { get; }
    uint Order { get; }
    StepLevel Level { get; }
    Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback, CancellationToken cancellationToken = default);
    void SetSequence(ISequenceT<TPayload> sequence);
}