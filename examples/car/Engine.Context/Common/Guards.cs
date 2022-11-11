using Ardalis.GuardClauses;
using DotCart.Context.Schemas;
using Engine.Contract.Schema;
using Exception = Engine.Context.Initialize.Exception;

namespace Engine.Context.Common;

public static class Guards
{
    public static IGuardClause EngineInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).HasFlag((int)EngineStatus.Initialized))
            throw Exception.New($"Engine {state.Id} is already initialized.");
        return guard;
    }

    public static IGuardClause StateIsNotInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)EngineStatus.Initialized))
            throw Exception.New($"engine {state.Id}  is not initialized");
        return guard;
    }


    public static IGuardClause EngineNotStarted(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)EngineStatus.Started))
            throw ChangeRpm.Exception.New("Engine has not started yet.");
        return guard;
    }
}