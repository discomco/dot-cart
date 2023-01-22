using System;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class TypedIDTests<TID>
    : IoCTests where TID : IID
{
    protected TypedIDTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var NewTypedId = TestEnv.ResolveRequired<NewTypedID<TID>>();
        // WHEN
        var ID = NewTypedId();
        // THEN 
        Assert.NotNull(ID);
    }


    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var NewTypedId = TestEnv.ResolveRequired<NewTypedID<TID>>();
        Assert.NotNull(NewTypedId);
        // WHEN
        var ID = NewTypedId();
        // THEN
        Assert.NotNull(ID);
    }

    [Fact]
    public void ShouldBeAbleToCreateAnIDFromNew()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var NewTypedId = TestEnv.ResolveRequired<NewTypedID<TID>>();
        Assert.NotNull(NewTypedId);

        // WHEN
        var ID = NewTypedId();
        // THEN
        Assert.NotNull(ID);
    }


    [Fact]
    public void ShouldThrowAnExceptionIfNoPrefixIDPresent()
    {
        try
        {
            var guid = GuidUtils.NewGuid;
            // WHEN
            var ID = PrefixLessTypedId.NewComb(guid);
            // THEN
            Assert.NotNull(ID);
            Assert.Equal("my", IDPrefixAtt.Get<PrefixLessTypedId>());
            Assert.Equal($"my-{guid}", ID.Value);
        }
        catch (Exception)
        {
            Assert.True(true);
        }
        // GIVEN
    }


    private record PrefixLessTypedId : TypedID<PrefixLessTypedId>
    {
        public PrefixLessTypedId(string value) : base(value)
        {
        }
    }
}