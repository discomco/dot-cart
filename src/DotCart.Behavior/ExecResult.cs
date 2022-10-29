using DotCart.Contract;

namespace DotCart.Behavior;


public interface IExecResult
{
    IFeedback Feedback { get; }
    IEnumerable<IEvt> Events { get; }
}

public record ExecResult(IFeedback Feedback, IEnumerable<IEvt> Events) : IExecResult
{
    public static ExecResult Empty => new (DotCart.Contract.Feedback.Empty, Array.Empty<IEvt>());
}