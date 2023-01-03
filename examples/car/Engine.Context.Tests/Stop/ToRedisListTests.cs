using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

[DocId(IDConstants.EngineListId)]
public class ToRedisListTests
    : ListProjectionTestsT<
        Context.Stop.Spoke,
        Context.Stop.ToRedisList,
        Schema.EngineList,
        Contract.Stop.Payload,
        EventMeta>
{
    public ToRedisListTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddStopSpoke();
    }
}