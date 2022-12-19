using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Context.Actors;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Actors;

public abstract class CmdHandlerTestsT<TID, TState, TCmd, TEvt, TPayload> : IoCTests
    where TState : IState
    where TID : IID
    where TCmd : ICmdB
    where TEvt: IEvtB
    where TPayload : IPayload
{
    protected static IDCtorT<TID> _newID;
    protected IAggregateStore _aggStore;
    protected TCmd _cmd;
    protected ICmdHandler _cmdHandler;
    protected Feedback _feedback;

    protected CmdCtorT<TCmd, TID, TPayload> _newCmd;
    protected PayloadCtorT<TPayload> _newPayload;
    protected StateCtorT<TState> _newState;


    public CmdHandlerTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
        TestEnv.Services
            .AddCmdHandler();
    }

    [Fact]
    public void ShouldResolveCmdCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newCmd = TestEnv.ResolveRequired<CmdCtorT<TCmd, TID, TPayload>>();
        // THEN
        Assert.NotNull(_newCmd);
    }

    [Fact]
    public void ShouldCreateCmd()
    {
        Assert.NotNull(_newCmd);
    }


    [Fact]
    public void ShouldResolveCmdHandler()
    {
        var ch = TestEnv.Resolve<ICmdHandler>();
        Assert.NotNull(ch);
    }

    [Fact]
    public void ShouldResolveAggregateStore()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var es = TestEnv.ResolveRequired<IAggregateStore>();
        // THEN
        Assert.NotNull(es);
    }

    [Fact]
    public void ShouldResolveDifferentHandlers()
    {
        // GIVEN
        var ch1 = TestEnv.Resolve<ICmdHandler>();
        // WHEN
        var ch2 = TestEnv.Resolve<ICmdHandler>();
        // THEN
        Assert.NotSame(ch1, ch2);
    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newState = TestEnv.ResolveRequired<StateCtorT<TState>>();
        // THEN
        Assert.NotNull(_newState);
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(TestEnv);
    }

    [Fact]
    public void ShouldResolvePayloadCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_newPayload);
    }

    [Fact]
    public async Task ShouldHandleCmd()
    {
        // GIVEN
        var cts = new CancellationTokenSource(3_000);
        Assert.NotNull(_cmdHandler);
        // WHEN
        _cmd = CreateCmd();
        // AND
        try
        {
            _feedback = await _cmdHandler.HandleAsync(_cmd, cts.Token);
        }
        catch (Exception e)
        {
            Output.WriteLine(e.InnerAndOuter());
            Assert.True(false);
        }

        // THEN
        Assert.NotNull(_feedback);
    }

    protected TCmd CreateCmd()
    {
        return _newCmd(_newID(), _newPayload());
    }


    protected override void Initialize()
    {
        _cmdHandler = TestEnv.ResolveRequired<ICmdHandler>();
        _aggStore = TestEnv.ResolveRequired<IAggregateStore>();
        _newState = TestEnv.ResolveRequired<StateCtorT<TState>>();
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        _newCmd = TestEnv.ResolveRequired<CmdCtorT<TCmd, TID, TPayload>>();
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
    }
}