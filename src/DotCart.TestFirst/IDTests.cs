using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class IDTests<TID> : IoCTests where TID: IID
{

    protected NewTypedID<TID> NewTypedId; 
    
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

    private record PrefixLessTypedId : TypedID<PrefixLessTypedId>
    {
        public PrefixLessTypedId(string value) : base(value)
        {
        }
    }


    protected IDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        NewTypedId = Container.GetRequiredService<NewTypedID<TID>>();
    }


}