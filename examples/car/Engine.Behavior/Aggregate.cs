using System.Text.Json.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using DotCart.Drivers.Mediator;
using DotCart.Drivers.Serilog;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static partial class Inject
{
    
    public static IServiceCollection AddStateCtor(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddSingleton(Engine.Ctor);
    }

    public static IServiceCollection AddEngineBehavior(this IServiceCollection services)
    {
        return services
            .AddInitializeBehavior()
            .AddStartBehavior()
            .AddChangeRpmBehavior()
            .AddStopBehavior();
    }

}

public static class Guards
{
    public static IGuardClause EngineInitialized(this IGuardClause guard, Engine state)
    {
        if (((int)state.Status).HasFlag((int)Schema.EngineStatus.Initialized))
            throw Initialize.Exception.New($"Engine {state.Id} is already initialized.");
        return guard;
    }

    public static IGuardClause EngineNotInitialized(this IGuardClause guard, Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)Schema.EngineStatus.Initialized))
            throw Initialize.Exception.New($"engine {state.Id}  is not initialized");
        return guard;
    }


    public static IGuardClause EngineNotStarted(this IGuardClause guard, Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)Schema.EngineStatus.Started))
            throw Start.Exception.New("Engine has not started yet.");
        return guard;
    }
}


[DbName("3")]
public record Engine : IState
{
    public static readonly StateCtorT<Engine> Ctor = () => new Engine();

    public Engine()
    {
        Details = new Schema.Details();
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
        Details = Schema.Details.New("New Engine");
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


[Name("engine:aggregate")]
public interface IEngineAggregateInfo: IAggregateInfoB
{
    
}

// public class Aggregate : AggregateT<Engine>
// {
//     public Aggregate(IExchange exchange, StateCtorT<Engine> newState) : base(exchange, newState)
//     {
//     }
// }