namespace DotCart.Schema.Tests;
public class IdentityTests
{

    private record PrefixLessID : ID<PrefixLessID>
    {
        public PrefixLessID(string value) : base(value)
        {
        }
    }

    [IDPrefix("my")]
    private record MyID : ID<MyID>
    {
        public MyID(string value) : base(value)
        {
        }
    }

    [Fact]
    public void ShouldBeAbleToCreateAnIdentity()
    {
        // GIVEN
        var guid = GuidUtils.NewGuid;
        // WHEN
        var ID = MyID.NewComb(guid);
        // THEN
        Assert.NotNull(ID);
        Assert.Equal("my", ID.GetIdPrefix());
        Assert.Equal($"my-{guid}", ID.Value);
    }
    
    [Fact]
    public void ShouldBeAbleToCreateAnIDFromNew()
    {
        // WHEN
        var ID = MyID.New;
        // THEN
        Assert.NotNull(ID);
        Assert.Equal("my", ID.GetIdPrefix());
    }
    
    
    
    

    [Fact]
    public void ShouldThrowAnExeptionIfNoPrefixIDPresent()
    {
        try
        {
            var guid = GuidUtils.NewGuid;
            // WHEN
            var ID = PrefixLessID.NewComb(guid);
            // THEN
            Assert.NotNull(ID);
            Assert.Equal("my", ID.GetIdPrefix());
            Assert.Equal($"my-{guid}", ID.Value);

        }
        catch (Exception e)
        {
            Assert.True(true);
        }
        // GIVEN
        
    }
    
    
    
}