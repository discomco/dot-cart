using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ListenerTestsT<TSpoke, TActor,TFactPayload> : ActorTestsT<TSpoke,TActor>
where TFactPayload: IPayload 
where TSpoke : ISpokeT<TSpoke> 
where TActor : IActorT<TSpoke>
{
    protected ListenerTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
    
}