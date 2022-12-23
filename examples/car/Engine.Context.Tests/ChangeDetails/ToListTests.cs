using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeDetails;

public class ToListTests : ProjectionTestsT<
    Context.ChangeDetails.Spoke, 
    Context.ChangeDetails.ToRedisList,
    Schema.EngineList,
    Behavior.ChangeDetails.IEvt>
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
            .AddChangeDetailsSpoke();
    }
}