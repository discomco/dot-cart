using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects;
using DotCart.Schema;
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
    protected IAggregateStore _aggregateStore;
    protected TResponder _responder;
    private ICmdHandler _cmdHandler;


    protected EffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

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
        var evt2State = Container.GetRequiredService<Evt2State<TState, TEvt>>();
        // THEN
        Assert.NotNull(evt2State);
    }
    protected override void Initialize()
    {
        _responder = Container.GetHostedService<TResponder>();
        _aggregateStore = Container.GetRequiredService<IAggregateStore>();
        _aggregate = Container.GetRequiredService<IAggregate>();
        _cmdHandler = Container.GetRequiredService<ICmdHandler>();
        _aggregateBuilder = Container.GetRequiredService<IAggregateBuilder>();
    }
}