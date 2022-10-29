using Ardalis.GuardClauses;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;

namespace DotCart.TestEnv.Engine.Behavior;

public static class Guards
{
    public static IGuardClause EngineInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).HasFlag((int)EngineStatus.Initialized))
            throw Initialize.Exception.New($"Engine {state.Id} is already initialized.");
        return guard;
    }

    public static IGuardClause EngineNotStarted(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)EngineStatus.Started))
            throw ThrottleUp.Exception.New("Engine has not started yet.");
        return guard;
    }
}