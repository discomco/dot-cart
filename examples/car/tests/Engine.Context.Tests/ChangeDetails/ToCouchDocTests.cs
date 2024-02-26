using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class ToCouchDocTests
    : ProjectionTestsT<Context.ChangeDetails.Spoke, ICouchDocDbInfo, Context.ChangeDetails.ToCouchDoc,
        Schema.Engine, Contract.ChangeDetails.Payload, MetaB, Schema.EngineID>
{
    public ToCouchDocTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddChangeDetailsSpoke();
    }
}