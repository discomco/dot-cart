using DotCart.TestKit;
using Engine.Contract.Tests.Initialize;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Start;

public class HopeTests: HopeTestsT<Contract.Schema.EngineID, Contract.Start.Hope, Contract.Start.Payload>
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
            .AddTestIDCtor()
            .AddTransient(_ => TestUtils.Start.PayloadCtor)
            .AddTransient(_ => TestUtils.Start.HopeCtor);

    }

    protected override string GetExpectedTopic()
    {
        return Contract.Start.Topics.Hope;
    }
}