using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Engine.Contract.ChangeRpm;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects.Engine;

public class ChangeRpmEffectsTests : EffectsTests<
    Common.Schema.Engine,
    IEvt,
    Cmd,
    Hope,
    Fact,
    ChangeRpm.Effects.Responder,
    IModelStoreDriver<Common.Schema.Engine>,
    ChangeRpm.Effects.ToMemDocProjection>
{
    public ChangeRpmEffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMemEventStore()
            .AddChangeRpmEffects();
    }
}