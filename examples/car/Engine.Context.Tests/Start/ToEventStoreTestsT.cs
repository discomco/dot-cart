using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

public class ToEventStoreTestsT
    : ToEventStoreTestsT<
        Contract.Start.Payload,
        Meta,
        IDocStore<Schema.Engine>>
{
    public ToEventStoreTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddStartSpoke();
    }
}