using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;

namespace DotCart.TestKit.Schema;

public static class Materials
{
    public static string[] Kinds = { "Wood", "Metal", "Rock", "Fabric" };

    public static string Random()
    {
        var ndx = System.Random.Shared.Next(0, Kinds.Length);
        return Kinds[ndx];
    }
}

public record ThePayload : IPayload
{
    [JsonConstructor]
    public ThePayload(string material, decimal weight, DateTime arrival)
    {
        Material = material;
        Weight = weight;
        Arrival = arrival;
    }

    public string Material { get; }
    public decimal Weight { get; }
    public DateTime Arrival { get; }
    public string Id { get; }

    public static ThePayload New(string material, decimal weight, DateTime arrival)
    {
        return new ThePayload(material, weight, arrival);
    }

    public static ThePayload Random()
    {
        return New(Materials.Random(),
            System.Random.Shared.Next(1, 100),
            DateTime.UtcNow);
    }
}