using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class AggregateTestsT<TID, TState, TTryCmd, TApplyEvt, TCmd, TEvt> : IoCTests
    where TID : IID
    where TState : IState
    where TApplyEvt : ApplyEvtT<TState, TEvt>
    where TTryCmd : TryCmdT<TCmd, TState>
    where TEvt : IEvtB
    where TCmd : ICmdB
{
    protected IAggregate? _agg;
    protected IAggregateBuilder? _builder;
    protected TID _ID;
    protected IDCtorT<TID> _newID;
    protected StateCtorT<TState>? _newState;
    protected Evt2State<TState,TEvt> _evt2State;

    protected AggregateTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveTryCmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var tryCmds = TestEnv.ResolveAll<ITry>();
        // THEN
        try
        {
            var tryCmd = tryCmds.FirstOrDefault(c => c.GetType() == typeof(TTryCmd));
            Assert.NotNull(tryCmd);
        }
        catch (Exception e)
        {
            Output.WriteLine(e.InnerAndOuter());
            Assert.True(false);
            throw;
        }
    }

    [Fact]
    public void ShouldResolveApplyEvt()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var applyEvts = TestEnv.ResolveAll<IApply>();
        // THEN
        try
        {
            var apply = applyEvts.First(x => x.GetType() == typeof(TApplyEvt));
            Assert.True(apply != null);
        }
        catch (Exception e)
        {
            Output.WriteLine(e.InnerAndOuter());
            Assert.True(false);
            throw;
        }
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newState = TestEnv.ResolveRequired<StateCtorT<TState>>();
        // THEN
        Assert.NotNull(_newState);
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
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(_newID);
        // WHEN
        var ID = _newID();
        // THEN
        Assert.NotNull(ID);
    }

    [Fact]
    public void ShouldResolveAggregate()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var agg = TestEnv.Resolve<IAggregate>();
        // THEN
        Assert.NotNull(agg);
        var state = agg.GetState();
        Assert.NotNull(state);
    }

    [Fact]
    public async Task ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2State = TestEnv.ResolveRequired<Evt2State<TState, TEvt>>();
        // THEN
        Assert.NotNull(_evt2State);
    }


    protected override void Initialize()
    {
        _builder = TestEnv.Resolve<IAggregateBuilder>();
        _newState = TestEnv.Resolve<StateCtorT<TState>>();
        _agg = _builder.Build();
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        _ID = _newID();
    }
}