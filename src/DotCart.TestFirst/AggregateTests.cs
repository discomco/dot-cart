using DotCart.Behavior;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class  AggregateTests<TID, TState> : IoCTests where TID :IID where TState :IState
{
    protected TID _ID;
    protected IAggregate? _agg;
    protected IAggregateBuilder? _builder;
    protected NewState<TState>? _newState;
    protected NewID<TID> _newID;

    protected AggregateTests(ITestOutputHelper output, IoCTestContainer container)
        : base(output, container)
    {
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _newID = Container.GetRequiredService<NewID<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }


    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _newState = Container.GetRequiredService<NewState<TState>>();
        // THEN
        Assert.NotNull(_newState);
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
        // WHEN
        var agg = Container.GetService<IAggregate>();
        // THEN
        Assert.NotNull(agg);
        var state = agg.GetState();
        Assert.NotNull(state);
    }


    protected override void Initialize()
    {
        _builder = Container.GetService<IAggregateBuilder>();
        _newState = Container.GetService<NewState<TState>>();
        _agg = _builder.Build();
        _newID = Container.GetRequiredService<NewID<TID>>();
        _ID = _newID();
    }

}