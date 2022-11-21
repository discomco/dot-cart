using System.Text.Json.Serialization;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;
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
        Details = new Contract.Schema.Details("New Engine");
        Status = Contract.Schema.EngineStatus.Unknown;
    }

    [JsonConstructor]
    public Engine(string id, Contract.Schema.EngineStatus status, Contract.Schema.Details details)
    {
        Id = id;
        Status = status;
        Details = details;
        Power = 0;
    }

    private Engine(string id)
    {
        Id = id;
        Status = Contract.Schema.EngineStatus.Unknown;
        Details = new Contract.Schema.Details("New Engine");
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id { get; set; }

    public Contract.Schema.EngineStatus Status { get; set; }
    public int Power { get; set; }
    public Contract.Schema.Details Details { get; set; }

    public static Engine New(string id, Contract.Schema.EngineStatus status, Contract.Schema.Details details)
    {
        return new Engine(id, status, details);
    }
}