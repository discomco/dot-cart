using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Engine.Contract.Tests.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Stop;

[Topic(Contract.Stop.Topics.Hope_v1)]
public class HopeTests : HopeTestsT<Contract.Schema.EngineID, Contract.Stop.Hope, Contract.Stop.Payload>
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
            .AddTransient(_ => TestUtils.Stop.PayloadCtor)
            .AddTransient(_ => TestUtils.Stop.HopeCtor);
    }
}