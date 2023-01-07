using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.Initialize;

[Topic(Contract.Initialize.Topics.Cmd_v1)]
public class CmdTests
    : CmdTestsT<Schema.EngineID, Contract.Initialize.Payload, Meta>
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
            .AddTransient(_ => TestUtils.Initialize.CmdCtor)
            .AddTransient(_ => TestUtils.Initialize.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}