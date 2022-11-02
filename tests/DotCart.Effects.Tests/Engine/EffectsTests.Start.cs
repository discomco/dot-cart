
using DotCart.TestEnv.Engine;

namespace DotCart.Effects.Tests.Engine;

public partial class EffectsTests
{
    [Fact]
    public void ShouldResolveStartResponder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var sr = Container.GetHostedService<Start.Responder>();
        // THEN
        Assert.NotNull(sr);
    }


    
}