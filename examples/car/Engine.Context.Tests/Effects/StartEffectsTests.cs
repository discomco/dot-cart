using DotCart.Context.Abstractions.Drivers;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Start;
using Engine.Contract.Start;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Effects;

public class StartEffectsTests : EffectsTests<
    Common.Schema.Engine,
    IEvt,
    Cmd,
    Hope,
    Fact,
    Actors.IResponder,
    IModelStore<Common.Schema.Engine>,
    Actors.IToRedisDoc
>
{
    public StartEffectsTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddMemEventStore()
            .AddStartActors();
    }
}