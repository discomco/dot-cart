using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behavior;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class AggregateTestsT<TID, TState, TPayload, TMeta> : IoCTests
    where TID : IID
    where TState : IState
    where TPayload : IPayload
    where TMeta : IMetaB
{
    protected IAggregate? _agg;
    protected IAggregateBuilder? _builder;
    protected Evt2Doc<TState, TPayload, TMeta> _evt2State;
    protected TID _ID;
    protected IDCtorT<TID> _newID;
    protected StateCtorT<TState>? _newState;

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
            var tryCmd = tryCmds.FirstOrDefault(c => c.GetType() == typeof(TryCmdT<TState, TPayload, TMeta>));
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
            var apply = applyEvts.First(x => x.GetType() == typeof(ApplyEvtT<TState, TPayload, TMeta>));
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
    public Task ShouldResolveAggregateBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        // THEN
        Assert.NotNull(aggBuilder);
        return Task.CompletedTask;
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
    public async Task ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2State = TestEnv.ResolveRequired<Evt2Doc<TState, TPayload, TMeta>>();
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