using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

public abstract class Evt2DocTestsT<TIEvt, TDocID, TDoc, TPayload, TMeta> : IoCTests
    where TIEvt : IEvtT<TPayload>
    where TPayload : IPayload
    where TMeta : IEventMeta
    where TDoc : IState
    where TDocID : IID
{
    protected EvtCtorT<TIEvt,TPayload,TMeta> _evtCtor;
    protected StateCtorT<TDoc> _docCtor;
    public IDCtorT<TDocID> _idCtor;
    protected Evt2State<TDoc,TIEvt> _evt2Doc;
    protected PayloadCtorT<TPayload> _payloadCtor;
    protected MetaCtorT<TMeta> _metaCtor;

    protected Evt2DocTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }
    
    [Fact]
    public void ShouldResolveEvtCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evtCtor = TestEnv
            .ResolveRequired<EvtCtorT<TIEvt, TPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evtCtor);
    }

    [Fact]
    public void ShouldResolveDocCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _docCtor = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        // THEN
        Assert.NotNull(_docCtor);
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _idCtor = TestEnv.ResolveRequired<IDCtorT<TDocID>>();
        // THEN
        Assert.NotNull(_idCtor);
    }

    [Fact]
    public void ShouldResolveEvt2DocFunc()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2Doc = TestEnv.ResolveRequired <Evt2State<TDoc, TIEvt>>();
        // THEN
        Assert.NotNull(_evt2Doc);
    }

    [Fact]
    public void ShouldResolvePayloadCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _payloadCtor = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_payloadCtor);
    }

    [Fact]
    public void ShouldResolveMetaCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _metaCtor = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        // THEN
        Assert.NotNull(_metaCtor);
    }

    [Fact]
    public void ShouldExecuteEvt2DocFunc()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _evt2Doc = TestEnv.ResolveRequired <Evt2State<TDoc, TIEvt>>();
        Assert.NotNull(_evt2Doc);
        _docCtor = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        Assert.NotNull(_docCtor);
        _evtCtor = TestEnv.ResolveRequired<EvtCtorT<TIEvt, TPayload, TMeta>>();
        Assert.NotNull(_evtCtor);
        _payloadCtor = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        Assert.NotNull(_payloadCtor);
        _idCtor = TestEnv.ResolveRequired<IDCtorT<TDocID>>();
        Assert.NotNull(_idCtor);
        _metaCtor = TestEnv.ResolveRequired<MetaCtorT<TMeta>>();
        // WHEN
        var ID = _idCtor();
        var payload = _payloadCtor();
        var meta = _metaCtor(ID.Id());
        var evt = _evtCtor(ID, payload, meta);
        var oldDoc = _docCtor();
        var newDoc = _evt2Doc(oldDoc, evt);
        Assert.True(IsValidProjection(oldDoc, newDoc, evt));
    }
    protected abstract bool IsValidProjection(TDoc oldDoc, TDoc newDoc, Event evt);

}