using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Drivers.Default;
using DotCart.TestFirst.Effects;
using DotCart.TestKit;
using Engine.Behavior;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class
    InitializeResponderTests : ResponderTestsT<
        IResponderT<Contract.Initialize.Hope, Behavior.Initialize.Cmd>,
        Contract.Initialize.Hope,
        Behavior.Initialize.Cmd
    >
{
    private IEncodedConnection _bus;

    public InitializeResponderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddDefaultDrivers<Behavior.Engine,IEngineSubscriptionInfo>()
            .AddSingleton(_ => A.Fake<IAggregateStore>())
            .AddInitializeSpoke();
    }

    [Fact]
    public override void ShouldResolveConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _bus = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(_bus);
    }
}