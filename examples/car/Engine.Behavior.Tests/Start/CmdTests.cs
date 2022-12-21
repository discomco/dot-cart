using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Start;

[Topic(Behavior.Start.Topics.Cmd_v1)]
public class CmdTests : CmdTestsT<Schema.EngineID, Behavior.Start.Cmd, Contract.Start.Payload>
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
            .AddTransient(_ => TestUtils.Start.CmdCtor)
            .AddTransient(_ => TestUtils.Start.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.IDCtor);
    }
}