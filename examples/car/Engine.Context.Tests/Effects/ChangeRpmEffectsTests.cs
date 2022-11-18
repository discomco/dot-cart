using DotCart.Context.Abstractions.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.ChangeRpm;
using Engine.Contract.ChangeRpm;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects;

public class ChangeRpmEffectsTests : EffectsTests<
    Common.Schema.Engine,
    IEvt,
    Cmd,
    Hope,
    Fact,
    ChangeRpm.Actors.IResponder,
    IModelStore<Common.Schema.Engine>,
    ChangeRpm.Actors.IToMemDoc>
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