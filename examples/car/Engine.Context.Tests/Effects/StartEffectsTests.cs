using DotCart.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Drivers.InMem;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Context.Common;
using Engine.Context.Start;
using Engine.Contract.Start;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Mappers = Engine.Context.Start.Mappers;

namespace Engine.Context.Tests.Effects;

public class StartEffectsTests : EffectsTests<
    IEvt,
    Fact,
    IModelStore<Common.Schema.Engine>>
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
            .AddCmdHandler()
            .AddEngineAggregate()
            .AddStartActors();
    }
}