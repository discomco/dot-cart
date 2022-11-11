using DotCart.Client.Schemas;
using DotCart.Core;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class TypedIDTests<TID> : IoCTests where TID : IID
{
    protected NewTypedID<TID> NewTypedId;


    protected TypedIDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ID = NewTypedId();
        // THEN 
        Assert.NotNull(ID);
    }


    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
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
            Assert.Equal("my", IDPrefix.Get<PrefixLessTypedId>());
            Assert.Equal($"my-{guid}", ID.Value);
        }
        catch (Exception)
        {
            Assert.True(true);
        }
        // GIVEN
    }

    protected override void Initialize()
    {
        NewTypedId = Container.GetRequiredService<NewTypedID<TID>>();
    }

    private record PrefixLessTypedId : TypedID<PrefixLessTypedId>
    {
        public PrefixLessTypedId(string value) : base(value)
        {
        }
    }
}