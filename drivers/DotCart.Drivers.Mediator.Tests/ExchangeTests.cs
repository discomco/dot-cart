using DotCart.Abstractions.Actors;
using DotCart.Context.Spokes;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace DotCart.Drivers.Mediator.Tests;

public class ExchangeTests
    : ActiveComponentTestsT<IExchange>
{
    private IConsumer1 _consumer1;
    private IConsumer2 _consumer2;
    private IProducer _producer;
    private IProjector _projector;
    private TheSpoke _spoke;
    private ISpokeBuilder<TheSpoke> _spokeBuilder;
    private IHostedService _spokeHost;

    public ExchangeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveSpokeAsHostedService()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var host = TestEnv.ResolveHosted<TheSpoke>();
        // THEN
        Assert.NotNull(host);
    }

    [Fact]
    public void ShouldResolveProducer()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _producer = TestEnv.ResolveRequired<IProducer>();
        // THEN
        Assert.NotNull(_producer);
    }

    [Fact]
    public void ShouldResolveConsumer1()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _consumer1 = TestEnv.ResolveRequired<IConsumer1>();
        // THEN
        Assert.NotNull(_consumer1);
    }

    [Fact]
    public void ShouldResolveConsumer2()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _consumer2 = TestEnv.ResolveRequired<IConsumer2>();
        // THEN
        Assert.NotNull(_consumer2);
    }

    [Fact]
    public void ShouldResolveSpokeBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spokeBuilder = TestEnv.ResolveRequired<ISpokeBuilder<TheSpoke>>();
        // THEN
        Assert.NotNull(_spokeBuilder);
    }

    [Fact]
    public void ShouldResolveSpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spoke = TestEnv.ResolveRequired<TheSpoke>();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public void ShouldBuildSpoke()
    {
        // GIVEN
        Assert.NotNull(_spokeBuilder);
        // WHEN
        _spoke = _spokeBuilder.Build();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public async Task ShouldActivateSpoke()
    {
        var ts = new CancellationTokenSource();
        // GIVEN
        Assert.NotNull(TestEnv);
        var host = TestEnv.ResolveHosted<TheSpoke>();
        Assert.NotNull(host);
        var executor = TestEnv.ResolveRequired<IHostExecutor>();
        Assert.NotNull(executor);

        // WHEN
        await executor.StartAsync(ts.Token).ConfigureAwait(false);
        _spoke = executor.Services.ToArray()[0] as TheSpoke;
        Assert.NotNull(_spoke);
        // THEN

        // WHEN
        await Task.Run(async () =>
            {
                Assert.Same(_spoke, executor.Services.ToArray()[0]);
                while (_spoke.Status != ComponentStatus.Active) Thread.Sleep(1);
                return Task.CompletedTask;
            }, ts.Token)
            .ConfigureAwait(false);

        Assert.Equal(ComponentStatus.Active, _spoke.Status);

        await Task.Run(async () =>
        {
            await Task.Delay(3, ts.Token).ConfigureAwait(false);
            ts.Cancel(); // THEN
            executor.StopAsync(ts.Token);
        }, ts.Token).ConfigureAwait(false);
        Thread.Sleep(2000);
        Assert.NotEqual(ComponentStatus.Active, _spoke.Status);
    }

    [Fact]
    public void ShouldResolveProjector()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _projector = TestEnv.Resolve<IProjector>();
        // THEN
        Assert.NotNull(_projector);
    }

    protected override void Initialize()
    {
        _producer = TestEnv.ResolveRequired<IProducer>();
        _consumer1 = TestEnv.ResolveRequired<IConsumer1>();
        _consumer2 = TestEnv.ResolveRequired<IConsumer2>();
        _spokeBuilder = TestEnv.ResolveRequired<ISpokeBuilder<TheSpoke>>();
        _spoke = _spokeBuilder.Build();
        _projector = TestEnv.Resolve<IProjector>();
    }


    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTestEnv();
    }
}