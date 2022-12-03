using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Initialize;

[Topic(Contract.Initialize.Topics.Hope_v1)]
public class HopeTests : HopeTestsT<Contract.Schema.EngineID, Contract.Initialize.Hope, Contract.Initialize.Payload>
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
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor)
            .AddTransient(_ => TestUtils.Initialize.HopeCtor);
    }
}