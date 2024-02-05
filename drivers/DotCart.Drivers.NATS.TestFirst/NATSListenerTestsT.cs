using DockTrace.TestKit;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSListenerTestsT<TPayload, TMeta>
    : NATSTestsB
    where TPayload : IPayload
    where TMeta : IMetaB
{
    protected NATSListenerTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    [Fact]
    public void PayloadShouldHaveFactTopicAttribute()
    {
        // GIVEN
        // WHEN
        var topic = FactTopicAtt.Get<TPayload>();
        // THEN
        Assert.NotEmpty(topic);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolveSubscriberHostedService()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var subscriber = TestEnv.ResolveHosted<NATSListener<TPayload>>();
        // THEN
        Assert.NotNull(subscriber);
    }


    [Fact]
    [Trait("Category", "Integration")]
    public async Task ShouldStartSubscribing()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var cts = new CancellationTokenSource(15_000);
        var executor = TestEnv.ResolveRequired<IHostExecutor>();
        // WHEN
        await executor.StartAsync(cts.Token).ConfigureAwait(false);
        // THEN
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolvePublisher()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var publisher = TestEnv.ResolveRequired<INATSEmitter<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(publisher);
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddNATSEmitter<TPayload, TMeta>()
            .AddNATSListener<TPayload>(async (fact, _) =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger>();
                logger.Information($"Processed fact {fact}");
            });
    }
}