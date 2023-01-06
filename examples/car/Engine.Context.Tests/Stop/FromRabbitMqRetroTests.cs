using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Stop;

public class FromRabbitMqRetroTests
: ListenerTestsT<
    Context.Stop.Spoke, 
    Context.Stop.FromRabbitMqRetro, 
    Contract.Stop.Payload>
{
    public FromRabbitMqRetroTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddStopSpoke();
    }
}