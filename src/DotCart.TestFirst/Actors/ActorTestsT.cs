using DotCart.Abstractions.Actors;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ActorTestsT<TActor> : ActiveComponentTestsT<TActor>
    where TActor : IActor
{
    protected TActor _actor;


    protected ActorTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        // WHEN
        // THEN
    }
}