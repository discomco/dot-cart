using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ToRedisDocTests : ProjectionTestsT<
    Context.ChangeRpm.Spoke,
    Context.ChangeRpm.ToRedisDoc,
    Schema.Engine,
    Contract.ChangeRpm.Payload,
    MetaB>
{
    public ToRedisDocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddChangeRpmSpoke();
    }
}