using DotCart.Abstractions.Behavior;
using DotCart.Actors;
using DotCart.Drivers.EventStoreDB;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Behavior;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class HopePipeBuilderTests
    : PipeBuilderTestsT<Context.ChangeDetails.IHopePipe, Contract.ChangeDetails.Payload>
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
            .AddCmdHandler()
            .AddESDBStore()
            .AddChangeDetailsBehavior()
            .AddChangeDetailsACLFuncs()
            .AddHopeInPipe<Context.ChangeDetails.IHopePipe, Contract.ChangeDetails.Payload, MetaB>();
    }
}