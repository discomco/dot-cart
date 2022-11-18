using DotCart.Context.Abstractions.Drivers;
using DotCart.TestKit;
using DotCart.TestKit.Schema;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class RequesterDriverTests : IoCTests
{
    protected IEncodedConnection _encodedConnection;
    protected IRequesterDriver<TheHope> _theRequester;

    [Fact]
    public void ShouldResolveEncodedConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _encodedConnection = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(_encodedConnection);
    }

    [Fact]
    public void ShouldResolveTheRequester()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theRequester = TestEnv.ResolveRequired<IRequesterDriver<TheHope>>();
        // THEN
        Assert.NotNull(_theRequester);
    }

    public RequesterDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
        _encodedConnection = TestEnv.ResolveRequired<IEncodedConnection>();
        _theRequester = TestEnv.ResolveRequired<IRequesterDriver<TheHope>>();
    }

    protected override void SetTestEnvironment()
    {
       
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddCoreNATS()
            .AddTransient<IRequesterDriver<TheHope>,  TheRequesterDriver>();
    }
}