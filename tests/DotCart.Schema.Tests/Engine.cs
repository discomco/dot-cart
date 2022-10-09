using System.Text.Json.Serialization;

namespace DotCart.Schema.Tests;


public record Engine : IState
{
    public static NewState<Engine> New = () => new Engine();

    public EngineID ID { get; set; }
    public EngineStatus Status { get; set; }

    [JsonConstructor]
    public Engine(EngineID id, EngineStatus status)
    {
        ID = id;
        Status = status;
    }
    private Engine()
    {}
}

