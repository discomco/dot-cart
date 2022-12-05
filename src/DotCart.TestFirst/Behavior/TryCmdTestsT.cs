using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;


namespace DotCart.TestFirst.Behavior;

public abstract class TryCmdTestsT<TTryCmd, TCmd, TState, TID, TPayload> : IoCTests 
    where TTryCmd: ITry<TCmd,TState> 
    where TCmd : ICmd
    where TID : IID
    where TPayload : IPayload
    where TState : IState
{
    protected IDCtorT<TID> _newID;
    protected CmdCtorT<TCmd,TID,TPayload> _newCmd;
    protected ITry<TCmd,TState> _newTryCmd;
    protected PayloadCtorT<TPayload> _newPayload;
    protected TState _validState;
    protected TState _invalidState;

    public TryCmdTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public async Task ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(_newID);
    }

    [Fact]
    public async Task ShouldResolvePayloadCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_newPayload);
    }

    [Fact]
    public async Task ShouldResolveCmdCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newCmd = TestEnv.ResolveRequired<CmdCtorT<TCmd,TID, TPayload>>();
        // THEN
        Assert.NotNull(_newCmd);
    }

    [Fact]
    public void ShouldResolveTryCmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newTryCmd = TestEnv.ResolveRequired<ITry<TCmd,TState>>();
        // THEN
        Assert.NotNull(_newTryCmd);
    }

    [Fact]
    public void ShouldBeOfTypeTTryCmd()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newTryCmd = TestEnv.ResolveRequired<ITry<TCmd,TState>>();
        // THEN
        Assert.NotNull(_newTryCmd);
        Assert.IsType<TTryCmd>(_newTryCmd);
    }
    
    [Fact]
    public async Task ShouldVerifyCmdAgainstValidState()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        Assert.NotNull(_newTryCmd);
        Assert.NotNull(_newCmd);
        Assert.NotNull(_newID);
        Assert.NotNull(_newPayload);
        Assert.NotNull(_validState);
        // WHEN
        var fbk = _newTryCmd.Verify(_newCmd(_newID(), _newPayload()), _validState);
        // THEN
        Assert.NotNull(fbk);
        Assert.True(fbk.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldVerifyCmdAgainstInValidState()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        Assert.NotNull(_newTryCmd);
        Assert.NotNull(_newCmd);
        Assert.NotNull(_newID);
        Assert.NotNull(_newPayload);
        Assert.NotNull(_invalidState);
        // WHEN
        var id = _newID();
        Assert.NotNull(id);
        var cmd = _newCmd(id, _newPayload());
        Assert.NotNull(cmd);
        var fbk = _newTryCmd.Verify(cmd, _invalidState);
        // THEN
        Assert.NotNull(fbk);
        Assert.False(fbk.IsSuccess);
    }

   
    protected abstract TState GetInValidState();
    protected abstract TState GetValidState();
    protected override void Initialize()
    {
        _newTryCmd = TestEnv.ResolveRequired<ITry<TCmd,TState>>();
        _newCmd = TestEnv.ResolveRequired<CmdCtorT<TCmd,TID, TPayload>>();
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        _invalidState = GetInValidState();
        _validState = GetValidState();
    }
}