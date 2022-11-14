using DotCart.Context.Abstractions;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Effects;

public abstract class ActorTestsT<TActor> : IoCTests where TActor : IActor
{
    protected TActor _actor;

    protected ActorTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveActor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _actor = TestEnv.GetRequiredService<TActor>();
        // THEN
        Assert.NotNull(_actor);
    }

    [Fact]
    public async Task ShouldActivateActor()
    {
        // var ts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var ts = new CancellationTokenSource();
        // GIVEN
        Assert.NotNull(_actor);

        // WHEN
        _actor.Activate(ts.Token);
        // THEN
        // WHEN
        Thread.Sleep(2);

        Assert.True(_actor.IsRunning);

        await Task.Run(async () =>
        {
            await Task.Delay(3, ts.Token);
            ts.Cancel(); // THEN
        }, ts.Token);

        Thread.Sleep(2);

        Assert.False(_actor.IsRunning);
    }


    protected override void Initialize()
    {
        _actor = TestEnv.GetRequiredService<TActor>();
    }
}