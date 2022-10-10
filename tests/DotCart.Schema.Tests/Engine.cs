using System.Text.Json.Serialization;

namespace DotCart.Schema.Tests;


public record Engine : IState
{
    public static NewState<Engine> New = () => new Engine();

    public string Id { get; set; }
    public EngineStatus Status { get; set; }

    [JsonConstructor]
    public Engine(string id, EngineStatus status)
    {
        Id = id;
        Status = status;
    }
    private Engine()
    {}
}

