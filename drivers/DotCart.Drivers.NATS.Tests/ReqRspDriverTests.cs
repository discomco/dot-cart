using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Clients;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.Mediator;
using DotCart.TestKit;
using DotCart.TestKit.Behavior;
using DotCart.TestKit.Schema;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class ReqRspDriverTests : IoCTests
{
    protected IEncodedConnection _encodedConnection;
    protected IDCtorT<TheID> _newID;
    protected IRequesterT<TheHope> _theRequester;
    protected IResponderT<TheHope, TheCmd> _theResponder;

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


    [Fact]
    public void ShouldResolveEncodedConnection()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _encodedConnection = TestEnv.ResolveRequired<IEncodedConnection>();
        // THEN
        Assert.NotNull(_encodedConnection);
    }

    [Fact]
    public void ShouldResolveTheRequester()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theRequester = TestEnv.ResolveRequired<IRequesterT<TheHope>>();
        // THEN
        Assert.NotNull(_theRequester);
    }

    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TheID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveTheResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theResponder = TestEnv.ResolveRequired<IResponderT<TheHope, TheCmd>>();
        // THEN
        Assert.NotNull(_theResponder);
    }


    [Fact]
    public async Task ShouldStartResponder()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var cts = new CancellationTokenSource(100_000);
        // WHEN
        _theResponder = TestEnv.ResolveRequired<IResponderT<TheHope, TheCmd>>();
        _theResponder.Activate(cts.Token);
        // THEN
        await Task.Delay(1000, cts.Token);
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
            .AddTheIDCtor()
            // .AddSingleton<ITheResponder, TheResponder>()
            // .AddSingleton<IResponderT2<TheHope, TheCmd>, TheResponder>()
            .AddTransient<IRequesterT<TheHope>, TheRequester>()
            .AddTransient<IResponderDriverT<TheHope>, TheResponderDriver>()
            .AddTransient<IResponderT<TheHope, TheCmd>, ResponderT<IResponderDriverT<TheHope>, TheHope, TheCmd>>()
            .AddTransient(_ => Mappers._hope2Cmd);
    }
}