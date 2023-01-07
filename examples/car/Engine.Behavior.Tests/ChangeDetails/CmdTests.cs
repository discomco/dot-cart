using DotCart.Abstractions.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

[Topic(Contract.ChangeDetails.Topics.Cmd_v1)]
public class CmdTests
    : CmdTestsT<
        Schema.EngineID,
        Contract.ChangeDetails.Payload,
        Meta>
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
            .AddTransient(_ => TestUtils.ChangeDetails.CmdCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor)
            .AddTransient(_ => TestUtils.Schema.DocIDCtor);
    }
}