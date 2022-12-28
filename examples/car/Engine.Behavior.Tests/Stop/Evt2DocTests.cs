using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

public class Evt2DocTests
: Evt2DocTestsT<
    Behavior.Stop.IEvt, 
    Schema.EngineID, 
    Schema.Engine, 
    Contract.Stop.Payload, 
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
            .AddTransient(_ => TestUtils.Stop.EvtCtor)
            .AddTransient(_ => TestUtils.Stop.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor)
            .AddTransient(_ => TestUtils.Schema.DocCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddStopProjectionFuncs();
    }
}