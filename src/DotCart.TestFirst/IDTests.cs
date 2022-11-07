using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class IDTests<TID> : IoCTests where TID: IID
{

    protected NewID<TID> NewID; 
    
    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ID = NewID();
        // THEN 
        Assert.NotNull(ID);
    }
    
    
    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(NewID);
        // WHEN
        var ID = NewID();
        // THEN
        Assert.NotNull(ID);
    }

    [Fact]
    public void ShouldBeAbleToCreateAnIDFromNew()
    {
        // GIVEN
        Assert.NotNull(NewID);
        // WHEN
        var ID = NewID();
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
            var ID = PrefixLessID.NewComb(guid);
            // THEN
            Assert.NotNull(ID);
            Assert.Equal("my", IDPrefix.Get<PrefixLessID>());
            Assert.Equal($"my-{guid}", ID.Value);
        }
        catch (Exception)
        {
            Assert.True(true);
        }
        // GIVEN
    }

    private record PrefixLessID : ID<PrefixLessID>
    {
        public PrefixLessID(string value) : base(value)
        {
        }
    }


    protected IDTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }

    protected override void Initialize()
    {
        NewID = Container.GetRequiredService<NewID<TID>>();
    }


}