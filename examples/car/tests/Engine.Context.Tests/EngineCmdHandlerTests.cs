using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Actors;
using DotCart.Behavior;
using DotCart.Drivers.EventStoreDB;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using Engine.Contract;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Schema = Engine.Contract.Schema;

namespace Engine.Context.Tests;

public abstract class EngineCmdHandlerTests<TPayload>
    : CmdHandlerTestsT<Schema.EngineID, Schema.Engine, TPayload, MetaB>
    where TPayload : IPayload
{
    public EngineCmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddESDBStore()
            .AddBaseBehavior<IEngineAggregateInfo, Schema.Engine, TPayload, MetaB>()
            .AddCmdHandler()
            .AddTestDocIDCtor()
            .AddRootDocCtors();
    }
}