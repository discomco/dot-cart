using DotCart.Abstractions.Actors;
using DotCart.Context.Actors;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Actors;

public class ThePipeBuilderTests : PipeBuilderTestsT<TheContext.IPipeInfo, TheContract.Payload>
{
    public ThePipeBuilderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient<IStepT<TheContext.IPipeInfo, TheContract.Payload>, TheActors.Step>()
            .AddTransient<IStepT<TheContext.IPipeInfo, TheContract.Payload>, TheActors.Step>()
            .AddPipeBuilder<TheContext.IPipeInfo, TheContract.Payload>();
    }
}