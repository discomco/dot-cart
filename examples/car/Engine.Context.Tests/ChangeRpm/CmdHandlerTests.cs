using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class CmdHandlerTests : EngineCmdHandlerTests<Behavior.ChangeRpm.Cmd, Behavior.ChangeRpm.Evt, Contract.ChangeRpm.Payload>
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
            .AddChangeRpmBehavior()
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.CmdCtor);
    }
}