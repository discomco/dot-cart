using DotCart.Abstractions.Drivers;
using DotCart.Context.Actors;
using DotCart.TestFirst;
using DotCart.TestKit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ActorTests : EffectsTests<
    Behavior.ChangeRpm.IEvt,
    Contract.ChangeRpm.Fact,
    IModelStore<Behavior.Engine>>
{
    public ActorTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddCmdHandler()
            .AddTransient(_ => A.Fake<IAggregateStore>())
            .AddTransient(_ => A.Fake<IModelStore<Behavior.Engine>>())
            .AddChangeRpmSpoke();
    }
}