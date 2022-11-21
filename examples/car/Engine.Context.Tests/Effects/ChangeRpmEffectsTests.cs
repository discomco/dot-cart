using DotCart.Abstractions.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects;

public class ChangeRpmEffectsTests : EffectsTests<
    IEvt,
    Contract.ChangeRpm.Fact,
    IModelStore<Common.Schema.Engine>>
{
    public ChangeRpmEffectsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMemEventStore()
            .AddChangeRpmActors();
    }
}