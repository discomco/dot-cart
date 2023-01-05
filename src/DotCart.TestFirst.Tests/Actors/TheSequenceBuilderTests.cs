using DotCart.Abstractions.Actors;
using DotCart.Context.Actors;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Tests.Actors;

public class TheSequenceBuilderTests : SequenceBuilderTestsT<TheContract.Payload>
{
    public TheSequenceBuilderTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient<IStepT<TheContract.Payload>, TheActors.Step>()
            .AddTransient<IStepT<TheContract.Payload>, TheActors.Step>()
            .AddSequenceBuilder<TheContract.Payload>();
    }
}