using DotCart.Core;
using DotCart.TestFirst.Actors;
using DotCart.TestKit;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Start;

[DocId(IDConstants.EngineListId)]
public class ToListTests 
    : ListProjectionTestsT<
        Context.Start.Spoke, 
        Context.Start.ToRedisList, 
        Schema.EngineList,
        Behavior.Start.IEvt> 
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
            .AddStartSpoke();
    }
}