using DotCart.Behavior;
using DotCart.TestEnv.Engine;

namespace DotCart.Effects.Tests.Engine;

public partial class EffectsTests 
{

    [Fact]
    public void ShouldResolveResponder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ir = Container.GetHostedService<Initialize.Responder>();
        // THEN
        Assert.NotNull(ir);
    }

    [Fact]
    public async Task ShouldRunInitializeResponder()
    {
        // GIVEN
        Assert.NotNull(_initializeResponder);
        Assert.NotNull(_aggregateStore);
        Assert.NotNull(_aggregate);
        var tokenSource = new CancellationTokenSource(2_000);
        var cancellationToken = tokenSource.Token;
        // WHEN
        await Task.Run(async () =>
        {
            await _initializeResponder.StartAsync(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.SpinWait(5);
                Output.WriteLine("Waiting");
            }
        }, cancellationToken);
        var aStore = Container.GetRequiredService<IAggregateStore>();
        var t = aStore.GetType();
    }
    
    

    
    
}