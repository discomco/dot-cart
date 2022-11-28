using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class CmdHandlerTests: EngineCmdHandlerTests<Behavior.ChangeRpm.Cmd, Contract.ChangeRpm.Payload>
{
    
    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {}

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddTransient(_ => Utils.ChangeRpm.RandomPayloadCtor)
            .AddTransient(_ => Utils.ChangeRpm.RandomCmdCtor);
    }
}