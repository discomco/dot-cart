using DotCart.Behavior;
using DotCart.TestEnv.Engine;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public partial class EffectsTests: IoCTests
{
    private Initialize.Responder _initializeResponder;
    private IAggregateStore _aggregateStore;
    private IAggregate _aggregate;

    [Fact]
    public void ShouldResolveAggregateStore()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var aggStore = Container.GetRequiredService<IAggregateStore>();
        // THEN
        Assert.NotNull(aggStore);
    }

    
    protected override void Initialize()
    {
        _initializeResponder = Container.GetHostedService<Initialize.Responder>();
        _aggregateStore = Container.GetRequiredService<IAggregateStore>();
        _aggregate = Container.GetRequiredService<IAggregate>();
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddInitializeEffects()
            .AddStartEffects();
    }

    public EffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }
}