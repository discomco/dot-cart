using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Context.Actors;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class HopePipeBuilderTests
    : PipeBuilderTestsT<
        Context.Initialize.IHopePipe,
        Contract.Initialize.Payload>
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
            .AddTransient(_ => A.Fake<ICmdHandler>())
            .AddInitializeACLFuncs()
            .AddHopeInPipe<Context.Initialize.IHopePipe, Contract.Initialize.Payload, Meta>();
    }
}