using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeRpm;

[Topic(Behavior.ChangeRpm.Topics.Cmd_v1)]
public class CmdTests : CmdTestsT<Schema.EngineID, Behavior.ChangeRpm.Cmd, Contract.ChangeRpm.Payload>
{
    public CmdTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient(_ => TestUtils.ChangeRpm.CmdCtor)
            .AddTransient(_ => TestUtils.ChangeRpm.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor);
    }
}