using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class ToRabbitMqTests
    : EmitterTestsT<Context.ChangeDetails.Spoke, Context.ChangeDetails.ToRabbitMq,
        Contract.ChangeDetails.Payload, MetaB, Schema.EngineID>
{
    public ToRabbitMqTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
        TestUtils.ChangeDetails
            .AddTestFuncs(services)
            .AddChangeDetailsSpoke();
    }
}