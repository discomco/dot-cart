using System.Text.Json.Serialization;

namespace DotCart.Drivers.NATS;

// ReSharper disable once InconsistentNaming
public class NATSSettings
{
    [JsonPropertyName("user")] public string? User { get; set; }
    [JsonPropertyName("password")] public string? Password { get; set; }
    [JsonPropertyName("uri")] public string? Uri { get; set; }
}