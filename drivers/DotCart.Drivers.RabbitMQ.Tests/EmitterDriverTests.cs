using DotCart.Abstractions.Drivers;
using DotCart.TestFirst.Drivers;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.RabbitMQ.Tests;

public class EmitterDriverTests : RabbitMqDriverTests<TheContract.IFact, TheContract.Payload, byte[]>
{
    public EmitterDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddTransient<IEmitterDriverT<TheContract.Payload, byte[]>, RMqEmitterDriverT<TheContract.IFact, TheContract.Payload>>()
            .AddSingletonRMq()
            .AddTheACLFuncs();
    }
}