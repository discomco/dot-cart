using DotCart.Abstractions.Drivers;
using DotCart.TestFirst;
using DotCart.TestKit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class ActorTests : EffectsTests<
    Behavior.Stop.IEvt,
    Contract.Stop.Fact,
    IModelStore<Behavior.Engine>>
{
    public ActorTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddTransient(_ => A.Fake<IAggregateStore>())
            .AddTransient(_ => A.Fake<IModelStore<Behavior.Engine>>())
            .AddStopSpoke();
    }
}