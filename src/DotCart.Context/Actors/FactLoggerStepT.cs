using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Actors;

public class FactLoggerStepT<TPipeInfo, TFactPayload> : StepT<TPipeInfo, TFactPayload>
    where TFactPayload : IPayload
    where TPipeInfo : IPipeInfoB
{
    public override async Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback,
        CancellationToken cancellationToken = default)
    {
        var feedback = Feedback.New(msg.AggId, previousFeedback, Name);
        Log.Information($"{AppFacts.Received} fact {FactTopicAtt.Get<TFactPayload>()}");
        return feedback;
    }

    protected override string GetName()
    {
        return NameAtt.StepName(Level, NameAtt.Get(this), FactTopicAtt.Get<TFactPayload>());
    }
}