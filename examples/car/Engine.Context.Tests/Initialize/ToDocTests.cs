using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ToDocTests : ProjectionTestsT<
    Context.Initialize.Spoke,
    Context.Initialize.ToRedisDoc,
    Behavior.Engine,
    Behavior.Initialize.IEvt>
{
    public ToDocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddInitializeSpoke();
    }
}