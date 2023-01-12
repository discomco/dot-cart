using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

public class Evt2ListTests
    : Evt2DocTestsT<Schema.EngineListID,
        Schema.EngineList,
        Contract.Stop.Payload,
        MetaB>
{
    public Evt2ListTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Schema.ListIDCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.Stop.PayloadCtor)
            .AddTransient(_ => TestUtils.Stop.EvtCtor)
            .AddTransient(_ => TestUtils.Stop.StartedListCtor)
            .AddStopProjectionFuncs();
    }
}