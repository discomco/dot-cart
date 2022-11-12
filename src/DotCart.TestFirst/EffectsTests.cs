using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class EffectsTests<
    TState,
    TEvt,
    TCmd,
    THope,
    TFact,
    TResponder,
    TReadModelStore,
    TToDocProjection
> : IoCTests
    where TState : IState
    where TEvt : IEvt
    where TCmd : ICmd
    where THope : IHope
    where TFact : IFact
    where TResponder : IReactor
{
    protected IAggregate _aggregate;
    protected IAggregateBuilder _aggregateBuilder;
    private ICmdHandler _cmdHandler;
    protected TResponder _responder;
    protected IAggregateStoreDriver AggregateStoreDriver;


    protected EffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public async Task ShouldStartResponder()
    {
        try
        {
            Assert.NotNull(_responder);
            Assert.NotNull(AggregateStoreDriver);
            Assert.NotNull(_aggregate);
            var tokenSource = new CancellationTokenSource(1000);
            var cancellationToken = tokenSource.Token;
            // WHEN
            await Task.Run(async () =>
            {
                await _responder.Activate(cancellationToken);
                while (!cancellationToken.IsCancellationRequested)
                {
                    Thread.SpinWait(5);
                    Output.WriteLine("Waiting");
                }
            }, cancellationToken);
            var aStore = Container.GetRequiredService<IAggregateStoreDriver>();
            var t = aStore.GetType();
        }
        catch (TaskCanceledException e)
        {
            Assert.True(true);
        }
        // GIVEN
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
    public void ShouldResolveCmdHandler()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var cmdHandler = Container.GetRequiredService<ICmdHandler>();
        // THEN
        Assert.NotNull(cmdHandler);
    }

    [Fact]
    public void ShouldResolveToDocProjection()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var toDoc = Container.GetRequiredService<TToDocProjection>();
        // THEN 
        Assert.NotNull(toDoc);
    }

    [Fact]
    public void ShouldResolveAggregateStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var aggStore = Container.GetRequiredService<IAggregateStoreDriver>();
        // THEN
        Assert.NotNull(aggStore);
    }

    [Fact]
    public void ShouldResolveHope2Cmd()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var h2c = Container.GetRequiredService<Hope2Cmd<TCmd, THope>>();
        // THEN
        Assert.NotNull(h2c);
    }

    [Fact]
    public void ShouldResolveEvt2Fact()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var e2f = Container.GetRequiredService<Evt2Fact<TFact, TEvt>>();
        // THEN
        Assert.NotNull(e2f);
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
        var ir = Container.GetRequiredService<TResponder>();
        // THEN
        Assert.NotNull(ir);
    }

    [Fact]
    public void ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var evt2State = Container.GetRequiredService<Evt2State<TState, TEvt>>();
        // THEN
        Assert.NotNull(evt2State);
    }

    protected override void Initialize()
    {
        _responder = Container.GetRequiredService<TResponder>();
        AggregateStoreDriver = Container.GetRequiredService<IAggregateStoreDriver>();
        _aggregate = Container.GetRequiredService<IAggregate>();
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
        _aggregateBuilder = Container.GetRequiredService<IAggregateBuilder>();
    }
}