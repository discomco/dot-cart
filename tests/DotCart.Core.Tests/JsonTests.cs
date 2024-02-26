using DotCart.Abstractions.Core;

namespace DotCart.Core.Tests;

public record TestRecord(string Id, string Name, string Description)
{
}

public class JsonTests
{
    [Fact]
    public void ShouldSerializeRecord()
    {
        // GIVEN
        var rec = new TestRecord("123", "Jimmy", "Falcon");
        // WHEN
        var json = rec.ToJson();
        // THEN
        Assert.NotEqual("{}", json);
    }

    [Fact]
    public void ShouldDeserializeRecord()
    {
        // GIVEN
        var rec = new TestRecord("123", "Jimmy", "Falcon");
        var json = rec.ToJson();
        // WHEN
        var des = json.FromJson<TestRecord>();
        // THEN
        Assert.Equivalent(rec, des);
    }
}