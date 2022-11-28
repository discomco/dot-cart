using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ResponderTestsT<TResponder, THope, TCmd> : IoCTests
    where TResponder : IResponder
    where THope : IHope
    where TCmd : ICmd
{
    private TResponder _responder;

    public ResponderTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveHope2Cmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var h2c = TestEnv.ResolveRequired<Hope2Cmd<TCmd, THope>>();
        // THEN
        Assert.NotNull(h2c);
    }

    [Fact]
    public abstract void ShouldResolveConnection();


    [Fact]
    public void ShouldResolveResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _responder = TestEnv.ResolveRequired<TResponder>();
        // THEN
        Assert.NotNull(_responder);
    }

    [Fact]
    public async Task ShouldStartResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _responder = TestEnv.ResolveRequired<TResponder>();
        try
        {
            Assert.NotNull(_responder);
            var tokenSource = new CancellationTokenSource(1000);
            var cancellationToken = tokenSource.Token;
            // WHEN
            await Task.Run(async () =>
            {
                await _responder.Activate(cancellationToken);
                while (!cancellationToken.IsCancellationRequested)
                {
                    Thread.SpinWait(5);
                    Output.WriteLine("Waiting");
                }
            }, cancellationToken);
        }
        catch (TaskCanceledException e)
        {
            Assert.True(true);
        }
    }
}