using DotCart.Context.Abstractions;
using DotCart.Contract.Dtos;

namespace DotCart.Context.Behaviors;

public interface IExecResult
{
    IFeedback Feedback { get; }
    IEnumerable<IEvt> Events { get; }
}

public record ExecResult(IFeedback Feedback, IEnumerable<IEvt> Events) : IExecResult
{
    public static ExecResult Empty => new(Contract.Dtos.Feedback.Empty, Array.Empty<IEvt>());
}