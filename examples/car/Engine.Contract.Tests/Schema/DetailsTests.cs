using DotCart.Core;

namespace Engine.Contract.Tests.Schema;

public class DetailsTests
{
    [Fact]
    public void ShouldDeserializeDetails()
    {
        var det = Contract.Schema.Details.New("John", "John Lennon");
        var serialized = det.ToJson();
        var deserialized = serialized.FromJson<Contract.Schema.Details>();
        Assert.Equal(det, deserialized);
    }
}