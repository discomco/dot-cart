using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Start;

[Topic(Contract.Start.Topics.Hope_v1)]
public class HopeTests : HopeTestsT<Contract.Schema.EngineID, Contract.Start.Payload>
{
    public HopeTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTestIDCtor()
            .AddTransient(_ => TestUtils.Start.PayloadCtor)
            .AddTransient(_ => TestUtils.Start.HopeCtor);
    }
}