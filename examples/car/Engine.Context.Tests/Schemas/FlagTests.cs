using DotCart.Context.Abstractions;
using DotCart.TestKit;
using Engine.Contract.Schema;
using Xunit.Abstractions;

namespace Engine.Context.Tests.Schemas;

public class FlagTests : OutputTests
{
    public FlagTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ShouldSetAFlag()
    {
        // GIVEN
        var es = (int)EngineStatus.Initialized;
        // WHEN
        es = es.SetFlag((int)EngineStatus.Started);
        var hasStarted = es.HasFlag((int)EngineStatus.Started);
        // THEN
        Assert.True(hasStarted);
    }

    [Fact]
    public void ShouldUnsetFlag()
    {
        // GIVEN
        var es = (int)EngineStatus.Initialized;
        es = es.SetFlag((int)EngineStatus.Started);
        // WHEN
        es = es.UnsetFlag((int)EngineStatus.Started);

        es = es.SetFlag((int)EngineStatus.Stopped);

        var HasNotStarted = !es.HasFlag((int)EngineStatus.Started);
        // THEN
        Assert.True(HasNotStarted);
        Output.WriteLine($"{(EngineStatus)es}");
    }

    [Fact]
    public void ShouldSetFlags()
    {
        // GIVEN
        var es = (int)EngineStatus.Initialized;
        // WHEN
        es = es.SetFlags((int)EngineStatus.Started, (int)EngineStatus.Stopped);
        var isStarted = es.HasFlag((int)EngineStatus.Started);
        var isStopped = es.HasFlag((int)EngineStatus.Stopped);
        // THEN
        Assert.True(isStarted);
        Assert.True(isStopped);
        Output.WriteLine($"{(EngineStatus)es}");
    }

    [Fact]
    public void ShouldUnsetFlags()
    {
        // GIVEN
        var es = (int)EngineStatus.Initialized;
        es = es.SetFlags((int)EngineStatus.Started, (int)EngineStatus.Overheated);
        Output.WriteLine($"{(EngineStatus)es}");
        // WHEN
        es = es.UnsetFlags((int)EngineStatus.Initialized, (int)EngineStatus.Overheated);
        var isInitialized = es.HasFlag((int)EngineStatus.Initialized);
        var isStarted = es.HasFlag((int)EngineStatus.Started);
        var isOverheated = es.HasFlag((int)EngineStatus.Overheated);
        // THEN
        Output.WriteLine($"{(EngineStatus)es}");
        Assert.True(isStarted);
        Assert.False(isInitialized);
        Assert.False(isOverheated);
    }
}