namespace DotCart.Context.Abstractions;

public interface ITopicMediator : ITopicPubSub<IEvt>, IDisposable
{
}