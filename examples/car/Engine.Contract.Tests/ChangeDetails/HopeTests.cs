using DotCart.Core;
using DotCart.TestFirst.Contract;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.ChangeDetails;

[Topic(Contract.ChangeDetails.Topics.Hope_v1)]
public class HopeTests : HopeTestsT<Contract.Schema.EngineID, Contract.ChangeDetails.Hope,
    Contract.ChangeDetails.Payload>
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
            .AddTransient(_ => TestUtils.ChangeDetails.HopeCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}