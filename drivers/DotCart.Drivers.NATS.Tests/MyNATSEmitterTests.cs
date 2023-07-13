using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.NATS.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class MyNATSEmitterTests
    : NATSEmitterTestsT<MyPayload, MetaB>
{
    public MyNATSEmitterTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        base.InjectDependencies(services);
        services
            .AddTransient<FactCtorT<MyPayload, MetaB>>(_ => FactT<MyPayload, MetaB>.New)
            .AddTransient<PayloadCtorT<MyPayload>>(_ => MyPayload.New)
            .AddTransient<MetaCtorT<MetaB>>(_ => _ => MetaB.New("MyMeta", ""));
    }
}
