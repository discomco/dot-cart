using DotCart.TestKit;
using Engine.Context.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize.Effects;

public class ToDocProjectionTests : IoCTests
{
    private Context.Initialize.Effects.IToRedisDoc _toRedisDoc;

    public ToDocProjectionTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldResolveToRedisDocProjection()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        _toRedisDoc = Container.GetRequiredService<Context.Initialize.Effects.IToRedisDoc>();
        // THEN
        Assert.NotNull(_toRedisDoc);
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
            .AddInitializedRedisProjections();
    }
}