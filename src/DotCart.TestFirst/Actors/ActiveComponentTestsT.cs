using DotCart.Abstractions.Actors;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ActiveComponentTestsT<TActiveComponent>
    : IoCTests
    where TActiveComponent : IActiveComponent
{
    protected ActiveComponentTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveActiveComponent()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ac = TestEnv.ResolveRequired<TActiveComponent>();
        // THEN
        Assert.NotNull(ac);
    }

    [Fact]
    public void ShouldBeNamedComponent()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var name = NameAtt.Get<TActiveComponent>();
        // THEN
        Assert.NotEmpty(name);
    }

    // TODO: Make this a test again
    [Fact]
    public async Task ShouldActivateComponent()
    {
        //var ts = new CancellationTokenSource();
        // GIVEN
        var ts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        Assert.NotNull(TestEnv);
        var ac = TestEnv.ResolveRequired<TActiveComponent>();
        Assert.NotNull(ac);
        // WHEN
        await ac.Activate(ts.Token);
        // THEN
//        Thread.Sleep(5000);
        Assert.True(ac.Status.HasFlag(ComponentStatus.Active));
        // await Task.Run(async () =>
        // {
        //     await Task.Delay(3, ts.Token);
        //     ts.Cancel(); // THEN
        // }, ts.Token);
        // Thread.Sleep(2);
        // Assert.False(ac.Status.HasFlag(ComponentStatus.Active));
    }
}