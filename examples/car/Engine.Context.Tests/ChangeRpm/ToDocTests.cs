using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ToDocTests : ProjectionTestsT<
    Context.ChangeRpm.Spoke,
    IModelStore<Behavior.Engine>,
    Context.ChangeRpm.ToRedisDoc,
    Behavior.Engine,
    Behavior.ChangeRpm.IEvt>
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
            .AddChangeRpmSpoke();
    }
}