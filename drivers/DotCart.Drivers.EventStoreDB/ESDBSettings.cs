using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using EventStore.Client;

namespace DotCart.Drivers.EventStoreDB;

public class ESDBSettings
    : EventStoreClientSettings
{
    public const string SectionId = "eventStoreDB";
    [JsonPropertyName("tlsCaFile")] public string TlsCaFile { get; set; } = "/usr/local/share/ca-certificates/ca.crt";
    [JsonPropertyName("isInsecure")] public bool IsInsecure { get; set; } = true;
    [JsonPropertyName("tlsVerifyCert")] public bool TlsVerifyCert { get; set; } = false;
    [JsonPropertyName("tls")] public bool Tls { get; set; } = false;
    [JsonPropertyName("keepAliveTimeout")] public int KeepAliveTimeout { get; set; } = 10000;
    [JsonPropertyName("keepAliveInterval")] public int KeepAliveInterval { get; set; } = 10000;
    [JsonPropertyName("maxDiscoverAttempts")] public int MaxDiscoverAttempts { get; set; } = 10;
    [JsonPropertyName("username")] public string Username { get; set; } = "admin";
    [JsonPropertyName("password")] public string Password { get; set; } = "changeit";
    [JsonPropertyName("uri")] public string Uri { get; set; } = "discover+esdb://localhost:2113?keepAliveTimeout=10000&keepAliveInterval=10000&tls=false";
}