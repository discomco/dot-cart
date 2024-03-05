using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Funcs = Engine.TestUtils.Initialize.Funcs;

namespace Engine.Behavior.Tests.Initialize;

public class Evt2ListTests
    : Evt2DocTestsT<Schema.EngineListID, Schema.EngineList, Contract.Initialize.Payload, MetaB>
{
    public Evt2ListTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
            .AddInitializeProjectionFuncs()
            .AddListIDCtor()
            .AddTransient(_ => TestUtils.Schema.EmptyListCtor)
            .AddTransient(_ => Funcs.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => Funcs.EvtCtor);
    }
}