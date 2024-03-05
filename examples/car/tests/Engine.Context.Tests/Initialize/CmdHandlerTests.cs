using DotCart.TestKit;
using Engine.Behavior;
using Engine.TestUtils.Initialize;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class CmdHandlerTests
    : EngineCmdHandlerTests<Contract.Initialize.Payload>
{
    public CmdHandlerTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddInitializeBehavior()
            .AddTransient(_ => Funcs.PayloadCtor)
            .AddTransient(_ => Funcs.CmdCtor);
    }
}