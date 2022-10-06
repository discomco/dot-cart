namespace DotCart.Schema.Tests;
public class IdentityTests
{
    [IDPrefix("my")]
    private record MyID : Identity<MyID>
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
}