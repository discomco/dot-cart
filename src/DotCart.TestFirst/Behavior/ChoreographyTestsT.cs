using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behavior;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class ChoreographyTestsT<TCmdPayload, TEvtPayload, TMeta> : IoCTests
    where TCmdPayload : IPayload
    where TEvtPayload : IPayload
    where TMeta : IMeta
{
    private IAggregate _agg;
    private IAggregateBuilder _aggBuilder;
    private Evt2Cmd<TCmdPayload, TEvtPayload, TMeta> _evt2Cmd;
    private IChoreography _rule;

    protected ChoreographyTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveEvt2Cmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2Cmd = TestEnv.ResolveRequired<Evt2Cmd<TCmdPayload, TEvtPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evt2Cmd);
    }

    [Fact]
    public async Task ShouldResolveChoreography()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _rule = TestEnv.ResolveAll<IChoreography>()
            .First(x => x.Name == NameAtt.ChoreographyName<TEvtPayload, TCmdPayload>());
        // THEN
        Assert.NotNull(_rule);
    }

    [Fact]
    public void ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        // THEN
        Assert.NotNull(_aggBuilder);
    }

    [Fact]
    public void ShouldKnowChoreography()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        Assert.NotNull(_aggBuilder);
        // WHEN
        _agg = _aggBuilder.Build();
        var isKnown = _agg.KnowsChoreography(NameAtt.ChoreographyName<TEvtPayload, TCmdPayload>());
        // THEN
        Assert.True(isKnown);
    }
}