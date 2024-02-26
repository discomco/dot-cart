using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

public class Evt2ListTests
    : Evt2DocTestsT<
        Schema.EngineListID,
        Schema.EngineList,
        Contract.ChangeDetails.Payload,
        MetaB>
{
    public Evt2ListTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
            .AddChangeDetailsProjectionFuncs()
            .AddListIDCtor()
            .AddTransient(_ => TestUtils.Schema.FilledListCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.MetaCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.EvtCtor);
    }
}