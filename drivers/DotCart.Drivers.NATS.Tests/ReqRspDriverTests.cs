using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.Mediator;
using DotCart.TestKit;
using DotCart.TestKit.Schema;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Xunit.Abstractions;

namespace DotCart.Drivers.NATS.Tests;

public class ReqRspDriverTests : IoCTests
{
    protected IEncodedConnection _encodedConnection;
    protected NewID<TheID> _newID;
    protected ITheResponder _responderActor;
    protected IRequesterDriverB _theRequesterDriver;
    protected IResponderDriverT<TheHope> _theResponderDriver;

    public ReqRspDriverTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveResponderActor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _responderActor = TestEnv.ResolveRequired<ITheResponder>();
        // THEN
        Assert.NotNull(_responderActor);
    }


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
    public void ShouldResolveTheRequesterDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theRequesterDriver = TestEnv.ResolveRequired<IRequesterDriverB>();
        // THEN
        Assert.NotNull(_theRequesterDriver);
    }

    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<NewID<TheID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolveTheResponderDriver()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _theResponderDriver = TestEnv.ResolveRequired<IResponderDriverT<TheHope>>();
        // THEN
        Assert.NotNull(_theResponderDriver);
    }


    [Fact]
    public async Task ShouldStartResponder()
    {
        // GIVEN
        Assert.NotNull(_responderActor);
        var cts = new CancellationTokenSource(100_000);
        // WHEN
        _responderActor.Activate(cts.Token);
        // THEN
        await Task.Delay(1000, cts.Token);
        Assert.True(_responderActor.Status == ComponentStatus.Active);
    }


    [Fact]
    public async Task ShouldPerformRequest()
    {
        ShouldStartResponder();
        // GIVEN
        Assert.NotNull(TestEnv);
        Assert.NotNull(_theRequesterDriver);
        Assert.NotNull(_newID);
        var ID = _newID();
        var theHope = TheHope.New(ID.Id(), ThePayload.Random());
        // WHEN
        var fbk = await _theRequesterDriver.RequestAsync(theHope);
        // THEN
        Assert.NotNull(fbk);
        if (!fbk.IsSuccess)
        {
            Output.WriteLine(fbk.ErrState.ToString());
            Assert.Fail("RequestAsync: not successful");
        }
        else
        {
            Assert.True(fbk.IsSuccess);
        }
    }

    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<NewID<TheID>>();
        _encodedConnection = TestEnv.ResolveRequired<IEncodedConnection>();
        _theRequesterDriver = TestEnv.ResolveRequired<IRequesterDriverB>();
        _responderActor = TestEnv.ResolveRequired<ITheResponder>();
    }

    protected override void SetTestEnvironment()
    {
    }


    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddCoreNATS()
            .AddSingletonExchange()
            .AddSingleton(_ => A.Fake<ICmdHandler>())
            .AddTheIDCtor()
            .AddSingleton<ITheResponder, TheResponder>()
            .AddTransient<IRequesterDriverB, TheRequesterDriver>()
            .AddTransient<IResponderDriverT<TheHope>, TheResponderDriver>()
            .AddTransient(_ => Mappers._hope2Cmd);
    }
}