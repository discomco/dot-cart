using DotCart.TestEnv.Engine;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public class InitializeEffectsTests : EffectsTests<
    Initialize.Responder,
    Evt2State<TestEnv.Engine.Schema.Engine, Initialize.Evt>,
    IStore<TestEnv.Engine.Schema.Engine>,
    Initialize.ToMemDocProjection
>
{
    public InitializeEffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddInitializeEffects();
    }
}