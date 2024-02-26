using DotCart.Logging;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.TestFirst;

public abstract class NATSTestsB
    : IoCTests
{




    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolveEncodedConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var conn = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(conn);
    }

    [Fact]
    public void ShouldResolveLogger()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var logger = TestEnv.ResolveRequired<ILogger>();
        // THEN
        Assert.NotNull(logger);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolveConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var conn = TestEnv.ResolveRequired<IConnection>();
        // THEN
        Assert.NotNull(conn);
    }


    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldResolveNATSClientConnectionFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var factory = TestEnv.ResolveRequired<INatsClientConnectionFactory>();
        // THEN
        Assert.NotNull(factory);
    }

    protected NATSTestsB(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddSeriloggersFromSettingsOnly()
            .AddCoreNATS(options =>
            {
                options.User = "a";
                options.Password = "a";
            });
    }
}