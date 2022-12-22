using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;


public abstract class PolicyTestsT<TPolicy, TEvt, TCmd> : IoCTests
    where TPolicy : IAggregatePolicy
    where TEvt : IEvtB
    where TCmd : ICmdB
{
    protected Evt2Cmd<TCmd, TEvt> _evt2Cmd;
    protected IExchange _exchange;
    protected IAggregatePolicy _policy;


    protected PolicyTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public async Task ShouldResolveEvt2Cmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2Cmd = TestEnv.ResolveRequired<Evt2Cmd<TCmd, TEvt>>();
        // THEN
        Assert.NotNull(_evt2Cmd);
    }

    [Fact]
    public async Task ShouldHaveName()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var name = NameAtt.Get(this);
        // THEN
        Assert.NotNull(name);
    }

    [Fact]
    public async Task ShouldPolicyHaveName()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var name = NameAtt.Get<TPolicy>();
        // THEN
        Assert.NotNull(name);
    }


    [Fact]
    public async Task ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.ResolveRequired<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
    }


    [Fact]
    public async Task ShouldResolvePolicy()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _policy = TestEnv.ResolveAll<IAggregatePolicy>()
            .First(x => NameAtt.Get(x) == NameAtt.Get<TPolicy>());
        // THEN
        Assert.NotNull(_policy);
    }
}