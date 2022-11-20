using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Context.Common.Schema;

public static class Inject
{
    public static IServiceCollection AddModelCtor(this IServiceCollection services)
    {
        return services
            .AddModelIDCtor()
            .AddSingleton(Engine.Ctor);
    }
}

[DbName("3")]
public record Engine : IState
{
    public static readonly NewState<Engine> Ctor = () => new Engine();

    public Engine()
    {
        Details = Details.New("New Engine");
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id { get; set; }

    public EngineStatus Status { get; set; }
    public int Power { get; set; }
    public Details Details { get; set; }

    public static Engine New(string id, EngineStatus status, Details details)
    {
        return new Engine(id, status, details);
    }
}