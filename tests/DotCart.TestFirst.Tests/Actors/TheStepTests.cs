using DotCart.Abstractions.Actors;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using DotCart.TestKit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Actors;

public class TheStepTests : StepTestsT<TheContext.IPipeInfo, TheActors.Step, TheContract.Payload>
{
    public TheStepTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient<IStepT<TheContext.IPipeInfo, TheContract.Payload>, TheActors.Step>();
    }
}