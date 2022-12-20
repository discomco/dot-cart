using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class CmdTestsT<TID, TCmd, TPayload> : IoCTests
    where TCmd : ICmdB
    where TID : IID
    where TPayload : IPayload
{
    protected CmdCtorT<TCmd, TID, TPayload> _newCmd;
    protected IDCtorT<TID> _newID;
    protected PayloadCtorT<TPayload> _newPayload;

    public CmdTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
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
    public async Task ShouldResolveCmdCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _newCmd = TestEnv.ResolveRequired<CmdCtorT<TCmd, TID, TPayload>>();
        // THEN
        Assert.NotNull(_newCmd);
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
    public async Task ShouldHaveTopic()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var expectedTopic = TopicAtt.Get(this);
        // WHEN
        var cmdTopic = TopicAtt.Get<TCmd>();
        // THEN
        Assert.Equal(expectedTopic, cmdTopic);
    }
}