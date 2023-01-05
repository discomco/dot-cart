using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Clients;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.TestKit;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class ReqRspDriverTests : IoCTests
{
    protected IEncodedConnection _encodedConnection;
    protected IDCtorT<TheSchema.ID> _newID;
    protected IRequesterT<TheContract.Payload> _theRequester;
    protected IResponderT<TheContract.Payload> _theResponder;

    public ReqRspDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    // [Fact]
    // public void ShouldResolveResponderActor()
    // {
    //     // GIVEN
    //     Assert.NotNull(TestEnv);
    //     // WHEN
    //     _responderActor = TestEnv.ResolveRequired<ITheResponder>();
    //     // THEN
    //     Assert.NotNull(_responderActor);
    // }

//    [Fact]
    public void ShouldResolveOptionsAction()
    {
        Assert.NotNull(TestEnv);
        var opt = TestEnv.ResolveRequired<Action<Options>>();
        Assert.NotNull(opt);
    }

//    [Fact]
    public void ShouldResolveConnectionFactory()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _encodedConnection = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(_encodedConnection);
    }

//    [Fact]
    public void ShouldResolveTheRequester()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theRequester = TestEnv.ResolveRequired<IRequesterT<TheContract.Payload>>();
        // THEN
        Assert.NotNull(_theRequester);
    }

    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TheSchema.ID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveTheResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theResponder = TestEnv.ResolveRequired<IResponderT<TheContract.Payload>>();
        // THEN
        Assert.NotNull(_theResponder);
    }


    // TODO: make this green
//    [Fact]
    public async Task ShouldStartResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var cts = new CancellationTokenSource(2_000);
        // WHEN
        _theResponder = TestEnv.ResolveRequired<IResponderT<TheContract.Payload>>();
        await _theResponder.Activate(cts.Token).ConfigureAwait(false);
        // THEN
        // while (!cts.Token.IsCancellationRequested)
        // {
        //     Thread.Sleep(1000);            
        // }
        Assert.True(_theResponder.Status == ComponentStatus.Active);
    }


    // [Fact]
    // public async Task ShouldPerformRequest()
    // {
    //     ShouldStartResponder();
    //     // GIVEN
    //     Assert.NotNull(TestEnv);
    //     Assert.NotNull(_theRequesterDriver);
    //     Assert.NotNull(_newID);
    //     var ID = _newID();
    //     var theHope = TheHope.New(ID.Id(), ThePayload.Random());
    //     // WHEN
    //     var fbk = await _theRequesterDriver.RequestAsync(theHope);
    //     // THEN
    //     Assert.NotNull(fbk);
    //     if (!fbk.IsSuccess)
    //     {
    //         Output.WriteLine(fbk.ErrState.ToString());
    //         Assert.Fail("RequestAsync: not successful");
    //     }
    //     else
    //     {
    //         Assert.True(fbk.IsSuccess);
    //     }
    // }

    protected override void Initialize()
    {
        // _newID = TestEnv.ResolveRequired<NewID<TheID>>();
        // _encodedConnection = TestEnv.ResolveRequired<IEncodedConnection>();
        // _theRequesterDriver = TestEnv.ResolveRequired<IRequesterT<TheHope>>();
        // _responderActor = TestEnv.ResolveRequired<ITheResponder>();
    }

    protected override void SetEnVars()
    {
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddCoreNATS()
            .AddSingletonExchange()
            .AddSingleton(_ => A.Fake<ICmdHandler>())
            .AddTransient(_ => A.Fake<IAggregateStore>())
            .AddTheIDCtor()
            .AddTransient(_ => TheSchema.Doc.Rand)
            .AddTransient<IRequesterT<TheContract.Payload>, TheRequester>()
            .AddTransient<IResponderDriverT<TheContract.Payload>, TheResponderDriver>()
            .AddTransient<
                IResponderT<TheContract.Payload>,
                ResponderT<TheSpoke, TheContract.Payload, TheContract.Meta>>()
            .AddSequenceBuilder<TheContract.Payload>()
            .AddTransient(_ => Mappers._hope2Cmd);
    }
}