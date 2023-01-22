using System;
using System.Threading;
using System.Threading.Tasks;
using DotCart.Abstractions.Actors;
using DotCart.Core;
using DotCart.TestKit;
using Xunit;
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

    [Fact]
    public async Task ShouldActivateComponent()
    {
        // GIVEN
        var ts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        Assert.NotNull(TestEnv);
        var ac = TestEnv.ResolveRequired<TActiveComponent>();
        Assert.NotNull(ac);
        // WHEN
        await ac.Activate(ts.Token);
        // THEN
        Assert.True(ac.Status.HasFlag(ComponentStatus.Active));
    }
}