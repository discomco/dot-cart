using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Initialize;

public class Evt2ListTests
    : Evt2DocTestsT<
        Behavior.Initialize.IEvt,
        Schema.EngineListID,
        Schema.EngineList,
        Contract.Initialize.Payload,
        EventMeta>
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
            .AddInitializeProjectionFuncs()
            .AddListIDCtor()
            .AddTransient(_ => TestUtils.Schema.EmptyListCtor)
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.Initialize.EvtCtor);
    }
}