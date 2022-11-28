using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ProjectionTestsT<TProjection, TState, TEvt> : IoCTests
    where TProjection : IProjectionB
    where TState : IState
    where TEvt : IEvt

{
    public ProjectionTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveProjection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var toDoc = TestEnv.ResolveRequired<TProjection>();
        // THEN 
        Assert.NotNull(toDoc);
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
}