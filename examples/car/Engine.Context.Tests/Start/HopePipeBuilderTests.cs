using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Context.Actors;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class HopePipeBuilderTests: PipeBuilderTestsT<Context.Start.IHopePipe, Contract.Start.Payload>
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
            .AddStartACLFuncs()
            .AddHopeInPipe<Context.Start.IHopePipe,Contract.Start.Payload, EventMeta>();
    }
}