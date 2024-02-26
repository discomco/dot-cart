using Ardalis.GuardClauses;
using DotCart.Abstractions.Core;

namespace Engine.Contract;


public static class Guards
{
    public static IGuardClause EngineInitialized(this IGuardClause guard, Contract.Schema.Engine state)
    {
        if (((int)state.Status).HasFlag((int)Schema.Engine.Flags.Initialized))
            throw Contract.Initialize.Exception.New($"Engine {state.Id} is already initialized.");
        return guard;
    }

    public static IGuardClause EngineNotInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)Schema.Engine.Flags.Initialized))
            throw Initialize.Exception.New($"engine {state.Id}  is not initialized");
        return guard;
    }


    public static IGuardClause EngineNotStarted(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)Schema.Engine.Flags.Started))
            throw Start.Exception.New("Engine is not started.");
        return guard;
    }

    public static IGuardClause EngineStopped(this IGuardClause guard, Schema.Engine state)
    {
        if (state.Status.HasFlagFast(Schema.Engine.Flags.Stopped))
            throw Start.Exception.New("Engine is stopped.");
        return guard;
    }
}