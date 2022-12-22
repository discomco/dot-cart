using DotCart.Abstractions.Drivers;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ActorTests : EffectsTests<
    Behavior.ChangeRpm.IEvt,
    Contract.ChangeRpm.Fact,
    IModelStore<Schema.Engine>>
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
            .AddTransient(_ => A.Fake<IModelStore<Schema.Engine>>())
            .AddChangeRpmSpoke();
    }
}