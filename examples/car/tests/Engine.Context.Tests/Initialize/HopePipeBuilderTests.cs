using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class HopePipeBuilderTests
    : PipeBuilderTestsT<Context.Initialize.IHopePipe, Contract.Initialize.Payload>
{
    public HopePipeBuilderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddInitializeSpoke();
        // .AddTransient(_ => A.Fake<ICmdHandler>())
        // .AddInitializeBehavior()
        // .AddInitializeACLFuncs()
        // .AddESDBStore()
        // .AddHopeInPipe<Context.Initialize.IHopePipe, Contract.Initialize.Payload, MetaB>();
    }
}