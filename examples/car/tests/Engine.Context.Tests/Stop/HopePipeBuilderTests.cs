using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class HopePipeBuilderTests
    : PipeBuilderTestsT<Context.Stop.IHopeInPipe, Contract.Stop.Payload>
{
    public HopePipeBuilderTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
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
            .AddStopSpoke();
        // .AddTransient(_ => A.Fake<ICmdHandler>())
        // .AddStopACLFuncs()
        // .AddHopeInPipe<Context.Stop.IHopeInPipe, Contract.Stop.Payload, MetaB>();
    }
}