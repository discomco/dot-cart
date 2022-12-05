using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Initialize;

[Topic(Behavior.Initialize.Topics.Evt_v1)]
public class EvtTests : EvtTestsT<
    Schema.EngineID, 
    Behavior.Initialize.Evt, 
    Contract.Initialize.Payload>
{
    public EvtTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
        
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Initialize.EvtCtor)
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor);
    }
}
