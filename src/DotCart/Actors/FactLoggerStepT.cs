using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;
using Serilog;

namespace DotCart.Actors;

public class FactLoggerStepT<TPipeInfo, TFactPayload>
    : StepT<TPipeInfo, TFactPayload>
    where TFactPayload : IPayload
    where TPipeInfo : IPipeInfoB
{
    public override async Task<IFeedback> ExecuteAsync(
        IDto msg,
        IFeedback? previousFeedback,
        CancellationToken cancellationToken = default)
    {
        var feedback = Feedback.New(msg.AggId, previousFeedback, Name);
        Log.Information(
            messageTemplate: "{f} fact {f}",
            AppFacts.Received,
            FactTopicAtt.Get<TFactPayload>());
        return feedback;
    }

    protected override string GetName()
    {
        return NameAtt2.StepName(
            level: Level,
            stepName: NameAtt.Get(this),
            payloadTopic: FactTopicAtt.Get<TFactPayload>());
    }
}