using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Initialize;

public class Evt2DocTests 
    : Evt2DocTestsT<
        Behavior.Initialize.IEvt, 
        Schema.EngineID, 
        Schema.Engine, 
        Contract.Initialize.Payload, 
        EventMeta>
{
    public Evt2DocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient(_ => TestUtils.Initialize.EvtCtor)
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor)
            .AddRootDocCtors()
            .AddInitializeProjectionFuncs();
    }
}