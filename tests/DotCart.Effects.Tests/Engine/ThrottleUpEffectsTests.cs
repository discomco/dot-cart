using DotCart.Drivers.InMem;
using DotCart.TestEnv.Engine;
using DotCart.TestFirst;
using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Effects.Tests.Engine;

public class ThrottleUpEffectsTests: EffectsTests<
    TestEnv.Engine.Schema.Engine,
    ThrottleUp.Evt,
    ThrottleUp.Cmd,
    ThrottleUp.Hope,
    ThrottleUp.Fact,
    ThrottleUp.Responder, 
    IStore<TestEnv.Engine.Schema.Engine>,
    ThrottleUp.ToMemDocProjection>
{
    public ThrottleUpEffectsTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void SetTestEnvironment()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddThrottleUpEffects();
    }
}