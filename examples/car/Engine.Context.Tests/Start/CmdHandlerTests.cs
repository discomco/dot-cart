using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class CmdHandlerTests : EngineCmdHandlerTests<Behavior.Start.Cmd, Contract.Start.Payload>
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
            .AddTransient(_ => TestUtils.Start.CmdCtor)
            .AddTransient(_ => TestUtils.Start.PayloadCtor);
    }
}