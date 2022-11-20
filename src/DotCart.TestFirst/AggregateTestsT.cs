using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class AggregateTestsT<TID, TState> : IoCTests where TID : IID where TState : IState
{
    protected IAggregate? _agg;
    protected IAggregateBuilder? _builder;
    protected TID _ID;
    protected NewID<TID> _newID;
    protected NewState<TState>? _newState;

    protected AggregateTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<NewID<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }


    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newState = TestEnv.ResolveRequired<NewState<TState>>();
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
    public void ShouldResolveBehavior()
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


    protected override void Initialize()
    {
        _builder = TestEnv.Resolve<IAggregateBuilder>();
        _newState = TestEnv.Resolve<NewState<TState>>();
        _agg = _builder.Build();
        _newID = TestEnv.ResolveRequired<NewID<TID>>();
        _ID = _newID();
    }
}