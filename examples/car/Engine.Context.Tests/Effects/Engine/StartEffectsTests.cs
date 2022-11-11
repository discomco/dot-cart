using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Start;
using Engine.Contract.Start;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects.Engine;

public class StartEffectsTests : EffectsTests<
    Common.Schema.Engine,
    IEvt,
    Cmd,
    Hope,
    Fact,
    Start.Effects.Responder,
    IModelStoreDriver<Common.Schema.Engine>,
    Start.Effects.ToMemDocProjection
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
            .AddMemEventStore()
            .AddStartEffects();
    }
}