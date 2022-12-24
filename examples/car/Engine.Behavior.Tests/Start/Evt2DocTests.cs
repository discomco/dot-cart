using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Start;

public class Evt2DocTests
    : Evt2DocTestsT<
        Behavior.Start.IEvt,
        Schema.EngineID,
        Schema.Engine,
        Contract.Start.Payload, EventMeta>
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
            .AddTransient(_ => TestUtils.Start.PayloadCtor)
            .AddTransient(_ => TestUtils.Start.EvtCtor)
            .AddRootDocCtors()
            .AddStartProjectionFuncs();
    }

    protected override bool IsValidProjection(Schema.Engine oldDoc, Schema.Engine newDoc, Event evt)
    {
        return newDoc.Status.HasFlagFast(Schema.EngineStatus.Started) 
               && !newDoc.Status.HasFlagFast(Schema.EngineStatus.Stopped);
    }
}