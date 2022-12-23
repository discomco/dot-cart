using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Initialize;

public class ToListTests : ProjectionTestsT<
    Context.Initialize.Spoke, 
    Context.Initialize.ToRedisList, 
    Schema.EngineList, 
    Behavior.Initialize.IEvt>
{
    public ToListTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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