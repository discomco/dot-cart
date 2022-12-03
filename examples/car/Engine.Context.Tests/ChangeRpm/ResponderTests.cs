using DotCart.Abstractions.Actors;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ResponderTests: ResponderTestsT<
    IResponderT<Contract.ChangeRpm.Hope, Behavior.ChangeRpm.Cmd>,
    Contract.ChangeRpm.Hope,
    Behavior.ChangeRpm.Cmd>
{
    private IEncodedConnection _bus;

    public ResponderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddChangeRpmSpoke();

    }

    [Fact]
    public override async Task ShouldResolveConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _bus = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(_bus);
    }

}