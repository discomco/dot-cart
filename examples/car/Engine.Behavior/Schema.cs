using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static partial class Inject
{
    public static IServiceCollection AddModelCtor(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddSingleton(Engine.Ctor);
    }
}

[DbName("3")]
public record Engine : IState
{
    public static readonly StateCtor<Engine> Ctor = () => new Engine();

    public Engine()
    {
        Details = new Schema.Details("New Engine");
        Status = Schema.EngineStatus.Unknown;
    }

    [JsonConstructor]
    public Engine(string id, Schema.EngineStatus status, Schema.Details details)
    {
        Id = id;
        Status = status;
        Details = details;
        Power = 0;
    }

    private Engine(string id)
    {
        Id = id;
        Status = Schema.EngineStatus.Unknown;
        Details = new Schema.Details("New Engine");
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id { get; set; }

    public Schema.EngineStatus Status { get; set; }
    public int Power { get; set; }
    public Schema.Details Details { get; set; }

    public static Engine New(string id, Schema.EngineStatus status, Schema.Details details)
    {
        return new Engine(id, status, details);
    }
}