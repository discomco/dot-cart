using Ardalis.GuardClauses;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.Behavior;

public static partial class Inject
{
    public static IServiceCollection AddStateCtor(this IServiceCollection services)
    {
        return services
            .AddIDCtor()
            .AddSingleton(Schema.Engine.Ctor);
    }

    public static IServiceCollection AddEngineBehavior(this IServiceCollection services)
    {
        return services
            .AddInitializeBehavior()
            .AddStartBehavior()
            .AddChangeRpmBehavior()
            .AddStopBehavior()
            .AddChangeDetailsBehavior();
    }
}

public static class Constants
{
    public const string Aggregate_v1 = "engine:aggregate:v1";
}

public static class Guards
{
    public static IGuardClause EngineInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).HasFlag((int)Schema.EngineStatus.Initialized))
            throw Initialize.Exception.New($"Engine {state.Id} is already initialized.");
        return guard;
    }

    public static IGuardClause EngineNotInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)Schema.EngineStatus.Initialized))
            throw Initialize.Exception.New($"engine {state.Id}  is not initialized");
        return guard;
    }


    public static IGuardClause EngineNotStarted(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)Schema.EngineStatus.Started))
            throw Start.Exception.New("Engine has not started yet.");
        return guard;
    }
}

[Name(Constants.Aggregate_v1)]
public interface IEngineAggregateInfo : IAggregateInfoB
{
}



