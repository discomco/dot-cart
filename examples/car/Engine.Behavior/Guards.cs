using Ardalis.GuardClauses;
using DotCart.Abstractions.Schema;
using Engine.Contract;

namespace Engine.Behavior;

public static class Guards
{
    public static IGuardClause EngineInitialized(this IGuardClause guard, Engine state)
    {
        if (((int)state.Status).HasFlag((int)Schema.EngineStatus.Initialized))
            throw Initialize.Exception.New($"Engine {state.Id} is already initialized.");
        return guard;
    }

    public static IGuardClause StateIsNotInitialized(this IGuardClause guard, Engine state)
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