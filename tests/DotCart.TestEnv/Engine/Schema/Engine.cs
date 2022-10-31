using System.Text.Json.Serialization;
using DotCart.Schema;

namespace DotCart.TestEnv.Engine.Schema;

public record struct Engine : IState
{
    public static NewState<Engine> Ctor = () => new Engine();

    public Engine()
    {
        Details = Details.New("New Engine");
        Id = EngineID.New.Value;
        Status = EngineStatus.Unknown;
    }

    [JsonConstructor]
    public Engine(string id, EngineStatus status, Details details)
    {
        Id = id;
        Status = status;
        Details = details;
        Power = 0;
    }

    private Engine(string id)
    {
        Id = id;
        Status = EngineStatus.Unknown;
        Details = Details.New("New Engine");
    }

    public string Id { get; set; }
    public EngineStatus Status { get; set; }
    public int Power { get; set; } = 0;
    public Details Details { get; set; }
}

public record Details(string Name = "new engine", string Description = "")
{
    public static Details New(string name, string description = "")
    {
        return new Details(name, description);
    }
}