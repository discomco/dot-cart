using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Engine.Contract;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests;

public class StatusTests : OutputTests
{
    public StatusTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ShouldSetAFlag()
    {
        // GIVEN
        var es = (int)Schema.EngineStatus.Initialized;
        // WHEN
        es = es.SetFlag((int)Schema.EngineStatus.Started);
        var hasStarted = es.HasFlag((int)Schema.EngineStatus.Started);
        // THEN
        Assert.True(hasStarted);
    }

    [Fact]
    public void ShouldUnsetFlag()
    {
        // GIVEN
        var es = (int)Schema.EngineStatus.Initialized;
        es = es.SetFlag((int)Schema.EngineStatus.Started);
        // WHEN
        es = es.UnsetFlag((int)Schema.EngineStatus.Started);

        es = es.SetFlag((int)Schema.EngineStatus.Stopped);

        var HasNotStarted = !es.HasFlag((int)Schema.EngineStatus.Started);
        // THEN
        Assert.True(HasNotStarted);
        Output.WriteLine($"{(Schema.EngineStatus)es}");
    }

    [Fact]
    public void ShouldSetFlags()
    {
        // GIVEN
        var es = (int)Schema.EngineStatus.Initialized;
        // WHEN
        es = es.SetFlags((int)Schema.EngineStatus.Started, (int)Schema.EngineStatus.Stopped);
        var isStarted = es.HasFlag((int)Schema.EngineStatus.Started);
        var isStopped = es.HasFlag((int)Schema.EngineStatus.Stopped);
        // THEN
        Assert.True(isStarted);
        Assert.True(isStopped);
        Output.WriteLine($"{(Schema.EngineStatus)es}");
    }

    [Fact]
    public void ShouldUnsetFlags()
    {
        // GIVEN
        var es = (int)Schema.EngineStatus.Initialized;
        es = es.SetFlags((int)Schema.EngineStatus.Started, (int)Schema.EngineStatus.Overheated);
        Output.WriteLine($"{(Schema.EngineStatus)es}");
        // WHEN
        es = es.UnsetFlags((int)Schema.EngineStatus.Initialized, (int)Schema.EngineStatus.Overheated);
        var isInitialized = es.HasFlag((int)Schema.EngineStatus.Initialized);
        var isStarted = es.HasFlag((int)Schema.EngineStatus.Started);
        var isOverheated = es.HasFlag((int)Schema.EngineStatus.Overheated);
        // THEN
        Output.WriteLine($"{(Schema.EngineStatus)es}");
        Assert.True(isStarted);
        Assert.False(isInitialized);
        Assert.False(isOverheated);
    }
}