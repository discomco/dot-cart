using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Behavior;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ToEventStoreTestsT<TPayload, TMeta, TReadModelStore>
    : IoCTests
    where TPayload : IPayload
    where TMeta : IMetaB
{
    protected IAggregateBuilder AggregateBuilder;
    protected ICmdHandler CmdHandler;
    protected IExchange Exchange;
    protected IAggregateStore AggregateStore;


    protected ToEventStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
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
    public void ShouldResolveEvt2Fact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var e2f = TestEnv.ResolveRequired<Evt2Fact<TPayload, TMeta>>();
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
    public void ShouldResolveAggregateStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var aggStore = TestEnv.ResolveRequired<IAggregateStore>();
        // THEN
        Assert.NotNull(aggStore);
    }


    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        Exchange = TestEnv.ResolveRequired<IExchange>();
        // THEN
        Assert.NotNull(Exchange);
    }

    protected override void Initialize()
    {
        AggregateStore = TestEnv.ResolveRequired<IAggregateStore>();
        CmdHandler = TestEnv.ResolveRequired<ICmdHandler>();
        AggregateBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        Exchange = TestEnv.ResolveRequired<IExchange>();
    }
}