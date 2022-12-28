using DotCart.Core;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

[DocId(IDConstants.EngineListId)]
public class ToRedisListTests
    : ListProjectionTestsT<Context.ChangeRpm.Spoke,
        Context.ChangeRpm.ToRedisList,
        Schema.EngineList,
        Behavior.ChangeRpm.IEvt>
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
            .AddChangeRpmSpoke();
    }
}