using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class FromRabbitMqRetroTests
: ListenerTestsT<Context.Initialize.Spoke,
    Context.Initialize.FromRabbitMqRetro,
    Contract.Initialize.Payload>
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
            .AddInitializeSpoke();
    }
}