using DotCart.Behavior;
using DotCart.Effects;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestKit;

public abstract class EffectsTests<TResponder, TEvt2State, TReadModelStore, TToDocProjection>: IoCTests 
    where TResponder: IReactor
{
    protected TResponder _responder;
    protected IAggregateStore _aggregateStore;
    protected IAggregate _aggregate;
    protected IAggregateBuilder _aggregateBuilder;

    
    [Fact]
    public async Task ShouldStartResponder()
    {
        // GIVEN
        Assert.NotNull(_responder);
        Assert.NotNull(_aggregateStore);
        Assert.NotNull(_aggregate);
        var tokenSource = new CancellationTokenSource(100);
        var cancellationToken = tokenSource.Token;
        // WHEN
        await Task.Run(async () =>
        {
            await _responder.StartAsync(cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.SpinWait(5);
                Output.WriteLine("Waiting");
            }
        }, cancellationToken);
        var aStore = Container.GetRequiredService<IAggregateStore>();
        var t = aStore.GetType();
    }

    
    [Fact]
    public void ShouldResolveReadModelStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var rms = Container.GetRequiredService<TReadModelStore>();
        // THEN
        Assert.NotNull(rms);
    }

    [Fact]
    public void ShouldResolveToDocProjection()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var toDoc = Container.GetHostedService<TToDocProjection>();
        // THEN 
        Assert.NotNull(toDoc);
    }

    
    
    [Fact]
    public void ShouldResolveAggregateStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var aggStore = Container.GetRequiredService<IAggregateStore>();
        // THEN
        Assert.NotNull(aggStore);
    }

    [Fact]
    public void ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var aggBuilder = Container.GetRequiredService<IAggregateBuilder>();
        // THEN 
        Assert.NotNull(aggBuilder);
    }
    
    [Fact]
    public void ShouldResolveResponder()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ir = Container.GetHostedService<TResponder>();
        // THEN
        Assert.NotNull(ir);
    }
    
    [Fact]
    public void ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var evt2State = Container.GetRequiredService<TEvt2State>();
        // THEN
        Assert.NotNull(evt2State);
    }


    


    
    protected override void Initialize()
    {
        _responder = Container.GetHostedService<TResponder>();
        _aggregateStore = Container.GetRequiredService<IAggregateStore>();
        _aggregate = Container.GetRequiredService<IAggregate>();
    }


    protected EffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }
}