using DotCart.Abstractions.Behavior;

namespace DotCart.Abstractions.Actors;

public interface ITopicMediator : ITopicPubSub<IEvtB>, IDisposable
{
}