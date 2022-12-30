using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

[Name(Behavior.ChangeDetails.OnInitialized_v1)]
public class ShouldChangeDetailsOnInitializedTests
    : ChoreographyTestsT<Behavior.Initialize.IEvt,
        Behavior.ChangeDetails.Cmd>
{
    public ShouldChangeDetailsOnInitializedTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output,
        testEnv)
    {
    }


    [Fact]
    public async Task ShouldRaiseOutputEvent()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var aggBuilder = TestEnv.ResolveRequired<IAggregateBuilder>();
        Assert.NotNull(aggBuilder);
        // AND
        var agg = aggBuilder.Build();
        var aggID = Schema.DocIDCtor();
        var initPld = TestUtils.Initialize.PayloadCtor();
        var initCmd = Behavior.Initialize.Cmd.New(aggID, initPld);
        // WHEN
        var fdbk = await agg.ExecuteAsync(initCmd);
        Assert.NotNull(fdbk);
        Assert.True(fdbk.IsSuccess);
        // THEN
        Assert.Equal(2, agg.UncommittedEvents.Count());
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
            .AddInitializeBehavior()
            .AddChangeDetailsBehavior();
    }
}