using DotCart.Context.Abstractions;
using DotCart.TestKit;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Effects;

public class ToDocProjectionTests : IoCTests
{
    private IExchange _exchange;
    private Context.Initialize.Effects.IToRedisDoc _toRedisDoc;

    public ToDocProjectionTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveExchange()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _exchange = TestEnv.GetRequiredService<IExchange>();
        // THEN
        Assert.NotNull(_exchange);
    }


    [Fact]
    public void ShouldResolveToRedisDocProjection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _toRedisDoc = TestEnv.GetRequiredService<Context.Initialize.Effects.IToRedisDoc>();
        // THEN
        Assert.NotNull(_toRedisDoc);
    }

    protected override void Initialize()
    {
        _exchange = TestEnv.GetRequiredService<IExchange>();
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddInitializedRedisProjections();
    }
}