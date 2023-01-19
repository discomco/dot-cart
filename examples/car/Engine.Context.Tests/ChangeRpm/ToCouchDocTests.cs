using DotCart.Abstractions.Behavior;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.ChangeRpm;

public class ToCouchDocTests
: ProjectionTestsT<Context.ChangeRpm.Spoke,
    Context.ICouchDocDbInfo,
    Context.ChangeRpm.ToCouchDoc,
    Schema.Engine,
    Contract.ChangeRpm.Payload,
    MetaB,
    Schema.EngineID>
{
    public ToCouchDocTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
            .AddChangeRpmSpoke();
    }
}