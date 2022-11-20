using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Effects;
using DotCart.TestKit;
using Engine.Context.Initialize;
using Engine.Contract.Initialize;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Effects;

public class InitializeResponderTests : ResponderTestsT<IResponderT<Hope, Cmd>, Hope, Cmd>
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
            .AddSingleton(_ => A.Fake<IAggregateStoreDriver>())
            .AddInitializeResponders();
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