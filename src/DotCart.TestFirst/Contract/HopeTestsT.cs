using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace Engine.Contract.Tests.Initialize;

 public abstract class HopeTestsT<TID, THope, TPayload> : IoCTests
    where TID : IID
    where THope : IHope<TPayload>
    where TPayload : IPayload

{
    protected IDCtorT<TID> _newID;
    protected PayloadCtorT<TPayload> _newPayload;
    protected HopeCtorT<THope,TPayload> _newHope;
    
    public HopeTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        Assert.NotNull(TestEnv);
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        Assert.NotNull(_newID);
    }

    [Fact]
    public void ShouldResolvePayloadCtor()
    {
        Assert.NotNull(TestEnv);
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        Assert.NotNull(_newPayload);
    }

    [Fact]
    public void ShouldResolveHopeCtor()
    {
        Assert.NotNull(TestEnv);
        _newHope = TestEnv.ResolveRequired<HopeCtorT<THope, TPayload>>();
        Assert.NotNull(_newHope);
    }


    [Fact]
    public void ShouldDeserializeHope()
    {
        Assert.NotNull(_newID);
        Assert.NotNull(_newHope);
        var aggId = _newID().Id();
        Assert.NotEmpty(aggId);
        var hope = _newHope(aggId, _newPayload());
        var serialized = hope.ToJson();
        var desHope = serialized.FromJson<THope>();
        Assert.Equal(hope, desHope);
    }


    protected override void Initialize()
    {
        _newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        _newHope = TestEnv.ResolveRequired<HopeCtorT<THope, TPayload>>();
    }
}