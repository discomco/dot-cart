using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.TestFirst;
using DotCart.TestKit;
using Engine.Contract;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ActorTests : EffectsTests<
    Behavior.ChangeRpm.IEvt,
    FactT<Contract.ChangeRpm.Payload>,
    IDocStore<Schema.Engine>>
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
            .AddTransient(_ => A.Fake<IDocStore<Schema.Engine>>())
            .AddChangeRpmSpoke();
    }
}