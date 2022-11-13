using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Initialize;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Effects;

public class InitializeEffectsTests : EffectsTests<
    Common.Schema.Engine,
    IEvt,
    Cmd,
    Hope,
    Fact,
    Context.Initialize.Effects.IResponder,
    IModelStore<Common.Schema.Engine>,
    Context.Initialize.Effects.IMemDocProjection
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
            .AddMemEventStore()
            .AddInitializeEffects();
    }
}