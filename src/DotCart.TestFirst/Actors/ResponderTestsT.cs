using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ResponderTestsT<TSpoke, TActor, TPayload, TMeta> : ActorTestsT<TSpoke, TActor>
    where TPayload : IPayload
    where TMeta : IEventMeta
    where TActor : IActorT<TSpoke>
    where TSpoke : ISpokeT<TSpoke>
{
    private IResponderT<TPayload> _responder;

    public ResponderTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveHope2Cmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var h2c = TestEnv.ResolveRequired<Hope2Cmd<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(h2c);
    }

    [Fact]
    public abstract Task ShouldResolveConnectionFactory();
}