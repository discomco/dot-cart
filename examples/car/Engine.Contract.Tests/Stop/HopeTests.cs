using DotCart.TestKit;
using Engine.Contract.Tests.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Stop;

public class HopeTests: HopeTestsT<Contract.Schema.EngineID, Contract.Stop.Hope, Contract.Stop.Payload> 
{
    public HopeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
    protected override void SetTestEnvironment()
    {
    }
    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddIDCtor()
            .AddTransient(_ => TestUtils.Stop.TestPayloadCtor)
            .AddTransient(_ => TestUtils.Stop.HopeCtor);
    }


    protected override string GetExpectedTopic()
    {
        return Contract.Stop.HopeTopic;
    }
}