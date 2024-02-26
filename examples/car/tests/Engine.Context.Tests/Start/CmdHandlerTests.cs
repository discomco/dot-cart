using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class CmdHandlerTests :
    EngineCmdHandlerTests<Contract.Start.Payload>
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
            .AddStartBehavior()
            .AddTransient(_ => TestUtils.Start.CmdCtor)
            .AddTransient(_ => TestUtils.Start.PayloadCtor);
    }
}