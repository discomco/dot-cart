using DotCart.Context.Abstractions;
using DotCart.Context.Spokes;
using DotCart.TestFirst.Effects;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace DotCart.Drivers.Mediator.Tests;

public class ExchangeTests : ActorTestsT<IExchange>
{
    private IActor<Spoke, Consumer1> _consumer1;
    private IActor<Spoke, Consumer2> _consumer2;
    private IActor<Spoke, Producer> _producer;
    private IProjector _projector;
    private ISpokeT<Spoke> _spoke;
    private ISpokeBuilder<ISpokeT<Spoke>> _spokeBuilder;
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
        var host = TestEnv.GetHostedService<Spoke>();
        // THEN
        Assert.NotNull(host);
    }

    [Fact]
    public void ShouldResolveProducer()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _producer = TestEnv.GetRequiredService<IActor<Spoke, Producer>>();
        // THEN
        Assert.NotNull(_producer);
    }

    [Fact]
    public void ShouldResolveConsumer1()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _consumer1 = TestEnv.GetRequiredService<IActor<Spoke, Consumer1>>();
        // THEN
        Assert.NotNull(_consumer1);
    }

    [Fact]
    public void ShouldResolveConsumer2()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _consumer2 = TestEnv.GetRequiredService<IActor<Spoke, Consumer2>>();
        // THEN
        Assert.NotNull(_consumer2);
    }

    [Fact]
    public void ShouldResolveSpokeBuilder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spokeBuilder = TestEnv.GetRequiredService<ISpokeBuilder<ISpokeT<Spoke>>>();
        // THEN
        Assert.NotNull(_spokeBuilder);
    }

    [Fact]
    public void ShouldResolveSpoke()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _spoke = TestEnv.GetRequiredService<ISpokeT<Spoke>>();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public void ShouldBuildSpoke()
    {
        // GIVEN
        Assert.NotNull(_spokeBuilder);
        // WHEN
        var spoke = _spokeBuilder.Build();
        // THEN
        Assert.NotNull(_spoke);
    }

    [Fact]
    public async Task ShouldActivateSpoke()
    {
        var ts = new CancellationTokenSource();
        // GIVEN
        Assert.NotNull(TestEnv);
        var host = TestEnv.GetHostedService<Spoke>();
        Assert.NotNull(host);
        var executor = TestEnv.GetRequiredService<IHostExecutor>();
        Assert.NotNull(executor);
        // WHEN
        await executor.StartAsync(ts.Token);
        // THEN
        // WHEN
        Thread.Sleep(50);

        Assert.True(_consumer1.IsRunning);
        Assert.True(_consumer2.IsRunning);
        Assert.True(_producer.IsRunning);


        await Task.Run(async () =>
        {
            await Task.Delay(3, ts.Token);
            ts.Cancel(); // THEN
        }, ts.Token);

        Thread.Sleep(2);

        Assert.False(_producer.IsRunning);
        Assert.False(_consumer1.IsRunning);
        Assert.False(_consumer2.IsRunning);
    }

    [Fact]
    public void ShouldResolveProjector()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _projector = TestEnv.GetService<IProjector>();
        // THEN
        Assert.NotNull(_projector);
    }

    protected override void Initialize()
    {
        _actor = TestEnv.GetRequiredService<IExchange>();
        _producer = TestEnv.GetRequiredService<IActor<Spoke, Producer>>();
        _consumer1 = TestEnv.GetRequiredService<IActor<Spoke, Consumer1>>();
        _consumer2 = TestEnv.GetRequiredService<IActor<Spoke, Consumer2>>();
        _spokeBuilder = TestEnv.GetRequiredService<ISpokeBuilder<ISpokeT<Spoke>>>();
        _spoke = TestEnv.GetRequiredService<ISpokeT<Spoke>>();
        _projector = TestEnv.GetService<IProjector>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddTestEnv();
    }
}