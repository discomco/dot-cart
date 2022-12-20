using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class CmdHandlerTests : EngineCmdHandlerTests<Behavior.Initialize.Cmd, Behavior.Initialize.IEvt,
    Contract.Initialize.Payload>
{
    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddInitializeBehavior()
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor)
            .AddTransient(_ => TestUtils.Initialize.CmdCtor);
    }
}