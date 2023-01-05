using DotCart.Abstractions.Behavior;
using DotCart.Context.Behavior;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests.ChangeDetails;

[Name(Behavior.ChangeDetails.OnInitialized_v1)]
public class ShouldChangeDetailsOnInitializedTests
    : ChoreographyTestsT<
        Contract.ChangeDetails.Payload,
        Contract.Initialize.Payload,
        EventMeta>
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
        var initCmd = Command.New<Contract.Initialize.Payload>(
            aggID,
            initPld.ToBytes(),
            EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), aggID.Id()).ToBytes()
        );
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