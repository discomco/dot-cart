using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Core;

namespace DotCart.TestKit;

public static class TheActors
{
    [GroupName(TheConstants.SubscriptionGroup)]
    [IDPrefix(TheConstants.IDPrefix)]
    public interface IProjectorInfo : IProjectorInfoB
    {
    }

    [Name(TheConstants.StepName)]
    [Order(0)]
    public class Step : StepT<TheContext.IPipeInfo, TheContract.Payload>
    {
        protected override string GetName()
        {
            return NameAtt.StepName(Level, NameAtt.Get(this), FactTopicAtt.Get<TheContract.Payload>());
        }

        public override async Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback,
            CancellationToken cancellationToken = default)
        {
            var feedback = Feedback.New(msg.AggId, previousFeedback, Name);


            return feedback;
        }
    }
}