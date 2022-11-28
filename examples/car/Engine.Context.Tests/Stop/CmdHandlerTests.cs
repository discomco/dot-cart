using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class CmdHandlerTests: EngineCmdHandlerTests<Behavior.Stop.Cmd, Contract.Stop.Payload>
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
                .AddTransient(_ => Engine.Utils.Stop.TestPayloadCtor)
                .AddTransient(_ => Engine.Utils.Stop.TestCmdCtor);
        
    }
}