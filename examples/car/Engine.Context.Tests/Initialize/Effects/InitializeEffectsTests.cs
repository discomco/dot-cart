using DotCart.Abstractions.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Initialize;
using Engine.Contract.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Effects;

public class InitializeEffectsTests : EffectsTests<
    IEvt,
    Fact,
    IModelStore<Common.Schema.Engine>>
{
    public InitializeEffectsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMemEventStore()
            .AddInitializeActors();
    }
}