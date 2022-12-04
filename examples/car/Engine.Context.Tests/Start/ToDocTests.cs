using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class ToDocTests : ProjectionTestsT<
    Context.Start.Spoke,
    IModelStore<Behavior.Engine>,
    Context.Start.ToRedisDoc,
    Behavior.Engine,
    Behavior.Start.Evt>
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
            .AddStartSpoke();
    }
}