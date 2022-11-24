using DotCart.Abstractions.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ToDocTests : IoCTests
{
    private IExchange _exchange;
    private Context.Initialize.IToRedisDoc _toRedisDoc;

    public ToDocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.ResolveRequired<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
    }


    [Fact]
    public void ShouldResolveToRedisDocProjection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _toRedisDoc = TestEnv.ResolveRequired<Context.Initialize.IToRedisDoc>();
        // THEN
        Assert.NotNull(_toRedisDoc);
    }

    protected override void Initialize()
    {
        _exchange = TestEnv.ResolveRequired<IExchange>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddInitializeSpoke();
    }
}