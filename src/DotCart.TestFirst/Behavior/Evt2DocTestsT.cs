using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Behavior;

/// <summary>
///     Evt2DocTestsT is intended to provide a base for testing functions that project an Event onto a certain Document
/// </summary>
/// <typeparam name="TIEvt">The Injection Discriminator (Injector) that uniquely identifies the event</typeparam>
/// <typeparam name="TDocID">The ID Type for the Document</typeparam>
/// <typeparam name="TDoc">The Document Type</typeparam>
/// <typeparam name="TPayload">The Payload type for the Spoke</typeparam>
/// <typeparam name="TMeta">The Metadata Type</typeparam>
public abstract class Evt2DocTestsT<
    TDocID,
    TDoc,
    TPayload,
    TMeta> : IoCTests
    where TPayload : IPayload
    where TMeta : IMetaB
    where TDoc : IState
    where TDocID : IID
{
    protected StateCtorT<TDoc> _docCtor;
    protected Evt2Doc<TDoc, TPayload, TMeta> _evt2Doc;
    protected Evt2DocValidator<TDoc, TPayload, TMeta> _evt2DocVal;
    protected EvtCtorT<TPayload, TMeta> _evtCtor;
    public IDCtorT<TDocID> _idCtor;
    protected MetaCtorT<TMeta> _metaCtor;
    protected PayloadCtorT<TPayload> _payloadCtor;

    protected Evt2DocTestsT(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveEvtCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evtCtor = TestEnv.ResolveRequired<EvtCtorT<TPayload, TMeta>>();
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
    public void ShouldResolveProjectionValidatorFunction()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        _evt2DocVal = TestEnv.ResolveRequired<Evt2DocValidator<TDoc, TPayload, TMeta>>();
        // THEN
        Assert.NotNull(_evt2DocVal);
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
        _evt2Doc = TestEnv.ResolveRequired<Evt2Doc<TDoc, TPayload, TMeta>>();
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
    public Task ShouldExecuteEvt2DocFunc()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        _evt2Doc = TestEnv.ResolveRequired<Evt2Doc<TDoc, TPayload, TMeta>>();
        Assert.NotNull(_evt2Doc);
        _docCtor = TestEnv.ResolveRequired<StateCtorT<TDoc>>();
        Assert.NotNull(_docCtor);
        _evtCtor = TestEnv.ResolveRequired<EvtCtorT<TPayload, TMeta>>();
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
        var evt = _evtCtor(ID, payload.ToBytes(), meta.ToBytes());
        var oldDoc = _docCtor();
        var newDoc = _evt2Doc(oldDoc, evt);
        _evt2DocVal = TestEnv.ResolveRequired<Evt2DocValidator<TDoc, TPayload, TMeta>>();
        Assert.NotNull(_evt2DocVal);
        var isValid = _evt2DocVal(oldDoc, newDoc, evt);
        Assert.True(isValid);
        return Task.CompletedTask;
    }
}