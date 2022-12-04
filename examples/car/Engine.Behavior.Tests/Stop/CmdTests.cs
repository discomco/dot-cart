using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Stop;

[Topic(Behavior.Stop.Topics.Cmd_v1)]
public class CmdTests: CmdTestsT<Schema.EngineID, Behavior.Stop.Cmd, Contract.Stop.Payload>
{
    public CmdTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void Initialize()
    {
        
    }

    protected override void SetTestEnvironment()
    {
        
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => TestUtils.Stop.CmdCtor)
            .AddTransient(_ => TestUtils.Stop.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor);
    }
}