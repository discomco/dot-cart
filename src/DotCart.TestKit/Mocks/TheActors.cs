using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Core;
using DotCart.Schema;
using Serilog;

namespace DotCart.TestKit.Mocks;

public static class TheActors
{
    [GroupName(TheConstants.SubscriptionGroup)]
    [IDPrefix(TheConstants.DocIDPrefix)]
    public interface IProjectorInfo
        : IProjectorInfoB;

    [Name(TheConstants.StepName)]
    [Order(0)]
    public class Step
        : StepT<TheContext.IPipeInfo, TheContract.Payload>
    {
        protected override string GetName()
        {
            return NameAtt2.StepName(
                Level,
                NameAtt.Get(this),
                FactTopicAtt.Get<TheContract.Payload>());
        }

        public override async Task<IFeedback> ExecuteAsync(IDto msg, IFeedback? previousFeedback,
            CancellationToken cancellationToken = default)
        {
            var feedback = Feedback.New(msg.AggId, previousFeedback, Name);
            Log.Information($"{AppVerbs.Executing} step [{msg}]");
            return feedback;
        }
    }
}