using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
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
    where TResponder : IActor
{
    protected IAggregate _aggregate;
    protected IAggregateBuilder _aggregateBuilder;
    protected ICmdHandler _cmdHandler;
    protected IExchange _exchange;
    protected TResponder _responder;
    protected IAggregateStoreDriver AggregateStoreDriver;


    protected EffectsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            var aStore = TestEnv.ResolveRequired<IAggregateStoreDriver>();
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
        Assert.NotNull(TestEnv);
        // WHEN
        var rms = TestEnv.ResolveRequired<TReadModelStore>();
        // THEN
        Assert.NotNull(rms);
    }

    [Fact]
    public void ShouldResolveCmdHandler()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var cmdHandler = TestEnv.ResolveRequired<ICmdHandler>();
        // THEN
        Assert.NotNull(cmdHandler);
    }

    [Fact]
    public void ShouldResolveToDocProjection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var toDoc = TestEnv.ResolveRequired<TToDocProjection>();
        // THEN 
        Assert.NotNull(toDoc);
    }

    [Fact]
    public void ShouldResolveAggregateStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var aggStore = TestEnv.ResolveRequired<IAggregateStoreDriver>();
        // THEN
        Assert.NotNull(aggStore);
    }

    [Fact]
    public void ShouldResolveHope2Cmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var h2c = TestEnv.ResolveRequired<Hope2Cmd<TCmd, THope>>();
        // THEN
        Assert.NotNull(h2c);
    }

    [Fact]
    public void ShouldResolveEvt2Fact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var e2f = TestEnv.ResolveRequired<Evt2Fact<TFact, TEvt>>();
        // THEN
        Assert.NotNull(e2f);
    }

    [Fact]
    public void ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        // THEN 
        Assert.NotNull(aggBuilder);
    }

    [Fact]
    public void ShouldResolveResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ir = TestEnv.ResolveRequired<TResponder>();
        // THEN
        Assert.NotNull(ir);
    }

    [Fact]
    public void ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var evt2State = TestEnv.ResolveRequired<Evt2State<TState, TEvt>>();
        // THEN
        Assert.NotNull(evt2State);
    }

    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.ResolveRequired<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
    }

    protected override void Initialize()
    {
        _responder = TestEnv.ResolveRequired<TResponder>();
        AggregateStoreDriver = TestEnv.ResolveRequired<IAggregateStoreDriver>();
        _aggregate = TestEnv.ResolveRequired<IAggregate>();
        _cmdHandler = TestEnv.ResolveRequired<ICmdHandler>();
        _aggregateBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        _exchange = TestEnv.ResolveRequired<IExchange>();
    }
}