using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ToDocTests : ProjectionTestsT<
    Context.Initialize.Spoke,
    IModelStore<Behavior.Engine>,
    Context.Initialize.ToRedisDoc,
    Behavior.Engine,
    Behavior.Initialize.Evt>
{
    public ToDocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddInitializeSpoke();
    }
}