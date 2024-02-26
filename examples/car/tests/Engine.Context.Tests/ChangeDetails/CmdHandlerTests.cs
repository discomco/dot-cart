using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class CmdHandlerTests
    : EngineCmdHandlerTests<Contract.ChangeDetails.Payload>
{
    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddChangeDetailsBehavior()
            .AddTransient(_ => TestUtils.ChangeDetails.CmdCtor)
            .AddTransient(_ => TestUtils.ChangeDetails.PayloadCtor);
    }
}