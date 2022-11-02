using DotCart.TestEnv.Engine;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public class InitializeEffectTests : EffectTests
{
    public InitializeEffectTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldResolveInitializeResponder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ir = Container.GetHostedService<Initialize.Responder>();
        // THEN
        Assert.NotNull(ir);
    }

    
    
}