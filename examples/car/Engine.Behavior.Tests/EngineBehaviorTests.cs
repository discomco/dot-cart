using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.TestFirst.Behavior;
using DotCart.TestKit;
using Engine.TestUtils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Schema = Engine.Contract.Schema;

namespace Engine.Behavior.Tests;

public class EngineBehaviorTests : FullBehaviorTestsT
{
    private IAggregate _agg;
    private IAggregateBuilder _builder;
    private Schema.EngineID _ID;
    private IDCtorT<Schema.EngineID> _newID;
    private StateCtorT<Schema.Engine> _newState;
    private IAggregatePolicy? _startPolicy;

    public EngineBehaviorTests(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldLoadEvents()
    {
        // GIVEN
        Assert.NotNull(_builder);
        _agg = _builder.Build();
        Assert.NotNull(_agg);
        _ID = _newID();
        // AND
        var events = ScenariosAndStreams.InitializeWithChangeRpmEvents(_ID);
        _agg.SetID(_ID);
        _agg.Load(events);
        // THEN
        Assert.Equal(events.Count() - 1, _agg.Version);
    }

    [Fact]
    public async Task ShouldExecuteInitializeCmd()
    {
        // GIVEN
        Assert.NotNull(_builder);
        _agg = _builder.Build();
        Assert.NotNull(_agg);
        // WHEN
        var ID = TestUtils.Schema.DocIDCtor();
        var details = Schema.Details.New("New Engine");
        var cmd = CmdT<Contract.Initialize.Payload, EventMeta>
            .New(
                ID,
                Contract.Initialize.Payload.New(details),
                TestUtils.Schema.MetaCtor(ID.Id()));
        _agg.SetID(cmd.AggregateID);
        var feedback = await _agg.ExecuteAsync(cmd);
        var state = (Schema.Engine)feedback.Payload;
        // THEN
        Assert.NotNull(feedback);
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());
        Assert.True(feedback.IsSuccess);
        var isInitialized = state.Status.HasFlag(Schema.EngineStatus.Initialized);
        Assert.True(isInitialized);
//        Thread.Sleep(1_000);
        state = (Schema.Engine)_agg.GetState();
        Output.WriteLine($"{state}");
    }


    // TODO: Debug this ShouldExecuteStartCmd
    [Fact]
    public async Task ShouldExecuteStartCmd()
    {
        // GIVEN
        await ShouldExecuteInitializeCmd();
        _ID = _newID();
        var startCmd = CmdT<Contract.Start.Payload, EventMeta>.New(
            _ID,
            Contract.Start.Payload.New,
            EventMeta.New(NameAtt.Get<IEngineAggregateInfo>(), _ID.Id())
        );
//        _agg.SetID(_ID);
        // WHEN
        var feedback = await _agg.ExecuteAsync(startCmd);
        var state = (Schema.Engine)feedback.Payload;
        // THEN
        if (!feedback.IsSuccess) Output.WriteLine(feedback.ErrState.ToString());
        Assert.NotNull(feedback);
        Assert.True(feedback.IsSuccess);
        Assert.True(((int)state.Status).HasAllFlags(
            (int)Schema.EngineStatus.Initialized,
            (int)Schema.EngineStatus.Started));
        Output.WriteLine($"{state}");
    }


    [Fact]
    public void ShouldResolveStartTryCmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var builder = TestEnv.ResolveRequired<IAggregateBuilder>();
        var agg = builder.Build();
        var topic = CmdTopicAtt.Get<Contract.Start.Payload>();
        var knowsIt = agg.KnowsTry(topic);
        // THEN
        Assert.True(knowsIt);
    }

    protected override void Initialize()
    {
        _builder = TestEnv.ResolveRequired<IAggregateBuilder>();
        _newState = TestEnv.ResolveRequired<StateCtorT<Schema.Engine>>();
        _agg = _builder.Build();
        _newID = TestEnv.ResolveRequired<IDCtorT<Schema.EngineID>>();
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddStopBehavior()
            .AddChangeDetailsBehavior()
            .AddEngineBehavior();
    }
}