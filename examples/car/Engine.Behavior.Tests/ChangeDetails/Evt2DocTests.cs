using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

public class Evt2DocTests
    : Evt2DocTestsT<
        Behavior.ChangeDetails.IEvt, 
        Schema.EngineID, 
        Schema.Engine, 
        Contract.ChangeDetails.Payload, 
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
            .AddTransient(_ => TestUtils.ChangeDetails.NewPayloadCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.EvtCtor)
            .AddTransient(_ => TestUtils.Schema.OldDetailsCtor)
            .AddTransient(_ => TestUtils.Schema.DocCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor)
            .AddChangeDetailsProjectionFuncs();
    }

    protected override bool IsValidProjection(Schema.Engine oldDoc, Schema.Engine newDoc, Event evt)
    {
        return oldDoc.Details.Name != newDoc.Details.Name
               || oldDoc.Details.Description != newDoc.Details.Description;
    }
}