using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

[DocId(IDConstants.EngineListId)]
public class ToRedisListTests
    : ListProjectionTestsT<Context.Initialize.Spoke, IRedisListDbInfo, Context.Initialize.ToRedisList,
        Schema.EngineList, Contract.Initialize.Payload, MetaB, Schema.EngineListID>
{
    public ToRedisListTests(ITestOutputHelper output, IoCTestContainer testEnv)
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
            .AddInitializeSpoke();
    }
}