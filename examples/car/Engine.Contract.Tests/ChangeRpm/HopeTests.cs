using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.ChangeRpm;

[Topic(Contract.ChangeRpm.Topics.Hope_v1)]
public class HopeTests : HopeTestsT<Contract.Schema.EngineID, Contract.ChangeRpm.Hope, Contract.ChangeRpm.Payload>
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
            .AddTransient(_ => TestUtils.ChangeRpm.HopeCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor);
    }
}