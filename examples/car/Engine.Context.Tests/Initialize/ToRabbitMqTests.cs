using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ToRabbitMqTests
    : EmitterTestsT<
        Context.Initialize.Spoke,
        Context.Initialize.ToRabbitMq,
        Contract.Initialize.Payload,
        Meta>
{
    public ToRabbitMqTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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