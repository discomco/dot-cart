using DotCart.Effects.Drivers;
using DotCart.TestEnv.Engine;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public class InitializeEffectsTests : EffectsTests<
    TestEnv.Engine.Schema.Engine,
    Initialize.Evt,
    Initialize.Cmd,
    Initialize.Hope,
    Initialize.Fact,
    Initialize.Responder,
    IModelStoreDriver<TestEnv.Engine.Schema.Engine>,
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