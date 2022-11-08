using DotCart.Effects.Drivers;
using DotCart.TestEnv.Engine;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public class StartEffectsTests : EffectsTests<
    TestEnv.Engine.Schema.Engine,
    Start.IEvt,
    Start.Cmd,
    Start.Hope,
    Start.Fact,
    Start.Responder,
    IModelStoreDriver<TestEnv.Engine.Schema.Engine>,
    Start.ToMemDocProjection
>
{
    public StartEffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddStartEffects();
    }
}