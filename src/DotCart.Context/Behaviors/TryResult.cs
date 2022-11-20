using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;

namespace DotCart.Context.Behaviors;

public interface ITryResult
{
    Feedback Feedback { get; }
    IEnumerable<IEvt> Events { get; }
}

public record TryResult(Feedback Feedback, IEnumerable<IEvt> Events) : ITryResult
{
    public static TryResult Empty => new(Feedback.Empty, Array.Empty<IEvt>());
}