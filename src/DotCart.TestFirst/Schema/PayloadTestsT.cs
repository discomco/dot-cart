using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class PayloadTestsT<TPayload>
    : IoCTests
    where TPayload : IPayload
{
    private PayloadCtorT<TPayload> _newPayload;
    private TPayload _payload;


    protected PayloadTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
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
    public void ShouldCreatePayload()
    {
        Assert.NotNull(TestEnv);
        // WHEN
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_newPayload);
        _payload = _newPayload();
        Assert.NotNull(_payload);
    }

    [Fact]
    public void ShouldSerializePayload()
    {
        // ASSERT
        Assert.NotNull(TestEnv);
        _newPayload = TestEnv.ResolveRequired<PayloadCtorT<TPayload>>();
        // THEN
        Assert.NotNull(_newPayload);
        _payload = _newPayload();
        Assert.NotNull(_payload);
        var serialized = _payload.ToJson();
        Assert.False(string.IsNullOrEmpty(serialized));
        var deserialized = serialized.FromJson<TPayload>();
        Assert.NotNull(deserialized);
        Assert.Equal(_payload, deserialized);
    }
}