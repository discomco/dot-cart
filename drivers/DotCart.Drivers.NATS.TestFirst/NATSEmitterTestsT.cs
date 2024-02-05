using DockTrace.TestKit;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSEmitterTestsT<TPayload, TMeta>
    : NATSTestsB
    where TPayload : IPayload
    where TMeta : IMetaB
{
    private bool _processedFact;

    protected NATSEmitterTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
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
    public void ShouldResolvePublisher()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var publisher = TestEnv.ResolveRequired<INATSEmitter<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(publisher);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolveSubscriber()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var subscriber = TestEnv.ResolveHosted<NATSListener<TPayload>>();
        // THEN
        Assert.NotNull(subscriber);
    }


    [Fact]
    public void ShouldResolveFactCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var factCtor = TestEnv.ResolveRequired<FactCtorT<TPayload, TMeta>>();
        // THEN
        Assert.NotNull(factCtor);
    }

    [Fact]
    public void ShouldResolvePayloadCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var payloadCtor = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(payloadCtor);
    }

    [Fact]
    public void ShouldResolveMetaCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var metaCtor = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        // THEN
        Assert.NotNull(metaCtor);
    }


    [Fact]
    [Trait("Category", "Integration")]
    public async Task ShouldPublishFact()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _processedFact = false;
        var cts = new CancellationTokenSource(20_000);
        var host = TestEnv.ResolveRequired<IHostExecutor>();
        var publisher = TestEnv.ResolveRequired<INATSEmitter<TPayload, TMeta>>();
        Assert.NotNull(publisher);
        await host.StartAsync(cts.Token).ConfigureAwait(false);
        var factCtor = TestEnv.ResolveRequired<FactCtorT<TPayload, TMeta>>();
        Assert.NotNull(factCtor);
        var payloadCtor = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        Assert.NotNull(payloadCtor);
        var metaCtor = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        Assert.NotNull(metaCtor);
        var fact = factCtor("", payloadCtor(), metaCtor(""));
        Thread.Sleep(2_000);
        // WHEN
        await publisher.EmitAsync(fact).ConfigureAwait(false);

        Thread.Sleep(2_000);
        // THEN
        Assert.True(_processedFact);
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddHostExecutor()
            .AddNATSListener<TPayload>(async (_, _) => { _processedFact = true; })
            .AddNATSEmitter<TPayload, TMeta>();
    }
}