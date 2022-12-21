using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class ResponderTestsT<THope, TCmd> : IoCTests
//    where TIResponder : IResponder
    where THope : IHopeB
    where TCmd : ICmdB
{
    private IResponderT<THope,TCmd> _responder;

    public ResponderTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveHope2Cmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var h2c = TestEnv.ResolveRequired<Hope2Cmd<TCmd, THope>>();
        // THEN
        Assert.NotNull(h2c);
    }

    [Fact]
    public abstract Task ShouldResolveConnection();


    [Fact]
    public async Task ShouldResolveResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        TestEnv.Services.AddTransient(_ => A.Fake<ICmdHandler>());
        // WHEN
        _responder = TestEnv.ResolveRequired<IResponderT<THope,TCmd>>();
        // THEN
        Assert.NotNull(_responder);
    }

    [Fact]
    public async Task ShouldStartResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // GIVEN
        TestEnv.Services.AddTransient(_ => A.Fake<ICmdHandler>());

        // WHEN
        _responder = TestEnv.ResolveRequired<IResponderT<THope,TCmd>>();
        try
        {
            Assert.NotNull(_responder);
            var tokenSource = new CancellationTokenSource(1_000);
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