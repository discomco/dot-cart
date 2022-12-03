using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ProjectionTestsT<TSpoke, TProjection, TState, TEvt> : IoCTests
    where TProjection : IActor<TSpoke>
    where TState : IState
    where TEvt : IEvt
    where TSpoke: ISpokeT<TSpoke>

{
    public ProjectionTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {}
    
    private IExchange _exchange;
    private IActor<TSpoke>? _projection;
    private Evt2State<TState,TEvt> _evt2State;


    [Fact]
    public void ShouldResolveProjection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _projection = TestEnv.ResolveActor<TSpoke, TProjection>();
        // THEN 
        Assert.NotNull(_projection);
    }

    [Fact]
    public void ShouldResolveEvt2State()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2State = TestEnv.ResolveRequired<Evt2State<TState, TEvt>>();
        // THEN
        Assert.NotNull(_evt2State);
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

}