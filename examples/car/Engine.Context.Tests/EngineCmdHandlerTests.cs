using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Drivers.EventStoreDB;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using Engine.Contract;
using Engine.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Schema = Engine.Contract.Schema;

namespace Engine.Context.Tests;

public abstract class EngineCmdHandlerTests<TCmd, TPayload>: CmdHandlerTestsT<Schema.EngineID, Behavior.Engine, TCmd, TPayload> 
    where TCmd : ICmd 
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
            .AddBaseBehavior()
            .AddCmdHandler()
            .AddTestIDCtor()
            .AddStateCtor();
    }
}