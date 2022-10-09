using System.Text.Json.Serialization;

namespace DotCart.TestKit;

public record Payload
{
    [JsonConstructor]
    public Payload(string material, decimal weight, DateTime arrival, string id)
    {
        Material = material;
        Weight = weight;
        Arrival = arrival;
        Id = id;
    }

    public string Material { get; }
    public decimal Weight { get; }
    public DateTime Arrival { get; }
    public string Id { get; }
}