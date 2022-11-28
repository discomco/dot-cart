namespace Engine.Contract.Tests.Initialize;

public class PayloadTests
{
    [Fact]
    public void ShouldDetailsBeRequired()
    {
        // GIVEN
        // WHEN
        var pl = new Contract.Initialize.Payload();
        // THEN
        Assert.NotNull(pl.Details);
    }
}