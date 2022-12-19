using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Context.Behaviors;
using DotCart.Drivers.EventStoreDB;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Schema = Engine.Contract.Schema;

namespace Engine.Context.Tests;

public abstract class
    EngineCmdHandlerTests<TCmd, TEvt, TPayload> : CmdHandlerTestsT<Schema.EngineID, Behavior.Engine, TCmd, TEvt, TPayload>
    where TCmd : ICmdB
    where TPayload : IPayload
    where TEvt : IEvtB
{
    public EngineCmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddESDBStore()
            .AddBaseBehavior<IEngineAggregateInfo, Behavior.Engine, TCmd, TEvt> ()
            .AddCmdHandler()
            .AddTestIDCtor()
            .AddStateCtor();
    }
}