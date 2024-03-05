using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Funcs = Engine.TestUtils.Initialize.Funcs;

namespace Engine.Behavior.Tests.Initialize;

public class Evt2DocTests
    : Evt2DocTestsT<Schema.EngineID, Schema.Engine, Contract.Initialize.Payload, MetaB>
{
    public Evt2DocTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => Funcs.EvtCtor)
            .AddTransient(_ => Funcs.PayloadCtor)
            .AddRootDocCtors()
            .AddInitializeProjectionFuncs();
    }
}