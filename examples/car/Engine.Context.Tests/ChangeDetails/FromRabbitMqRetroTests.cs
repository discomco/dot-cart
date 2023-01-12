using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class FromRabbitMqRetroTests
    : RetroListenerTestsT<
        Context.ChangeDetails.Spoke,
        Context.ChangeDetails.FromRabbitMqRetro,
        Contract.ChangeDetails.Payload,
        MetaB,
        Context.ChangeDetails.IRetroPipe,
        Schema.EngineID>
{
    public FromRabbitMqRetroTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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