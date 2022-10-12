using System.Text.Json.Serialization;

namespace DotCart.Schema.Tests;


public record struct Engine : IState
{
    public static NewState<Engine> Ctor = () => new Engine();

    public string Id { get; set; }
    public EngineStatus Status { get; set; }

    public Details Details { get; }

    [JsonConstructor]
    public Engine(string id, EngineStatus status, Details details)
    {
        Id = id;
        Status = status;
        Details = details;
    }

    private Engine(string id)
    {
        Id = id;
        Status = EngineStatus.Unknown;
        Details = Details.New("New Engine");
    }
}

public record Details(string Name, string Description="")
{
    public string Name { get; }
    public string Description { get;  }
    public static Details New(string name, string description = "") => new(name, description);

}

