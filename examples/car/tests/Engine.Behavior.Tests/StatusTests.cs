using DotCart.Abstractions.Core;
using DotCart.TestKit;
using Engine.Contract;
using Xunit.Abstractions;

namespace Engine.Behavior.Tests;

public class StatusTests
    : OutputTests
{
    public StatusTests(ITestOutputHelper output)
        : base(output)
    {
    }

    [Fact]
    public void ShouldSetAFlag()
    {
        // GIVEN
        var es = (int)Schema.Engine.Flags.Initialized;
        // WHEN
        es = es.SetFlag((int)Schema.Engine.Flags.Started);
        var hasStarted = es.HasFlag((int)Schema.Engine.Flags.Started);
        // THEN
        Assert.True(hasStarted);
    }

    [Fact]
    public void ShouldUnsetFlag()
    {
        // GIVEN
        var es = (int)Schema.Engine.Flags.Initialized;
        es = es.SetFlag((int)Schema.Engine.Flags.Started);
        // WHEN
        es = es.UnsetFlag((int)Schema.Engine.Flags.Started);

        es = es.SetFlag((int)Schema.Engine.Flags.Stopped);

        var HasNotStarted = !es.HasFlag((int)Schema.Engine.Flags.Started);
        // THEN
        Assert.True(HasNotStarted);
        Output.WriteLine($"{(Schema.Engine.Flags)es}");
    }

    [Fact]
    public void ShouldSetFlags()
    {
        // GIVEN
        var es = (int)Schema.Engine.Flags.Initialized;
        // WHEN
        es = es.SetFlags((int)Schema.Engine.Flags.Started, (int)Schema.Engine.Flags.Stopped);
        var isStarted = es.HasFlag((int)Schema.Engine.Flags.Started);
        var isStopped = es.HasFlag((int)Schema.Engine.Flags.Stopped);
        // THEN
        Assert.True(isStarted);
        Assert.True(isStopped);
        Output.WriteLine($"{(Schema.Engine.Flags)es}");
    }

    [Fact]
    public void ShouldUnsetFlags()
    {
        // GIVEN
        var es = (int)Schema.Engine.Flags.Initialized;
        es = es.SetFlags((int)Schema.Engine.Flags.Started, (int)Schema.Engine.Flags.Overheated);
        Output.WriteLine($"{(Schema.Engine.Flags)es}");
        // WHEN
        es = es.UnsetFlags((int)Schema.Engine.Flags.Initialized, (int)Schema.Engine.Flags.Overheated);
        var isInitialized = es.HasFlag((int)Schema.Engine.Flags.Initialized);
        var isStarted = es.HasFlag((int)Schema.Engine.Flags.Started);
        var isOverheated = es.HasFlag((int)Schema.Engine.Flags.Overheated);
        // THEN
        Output.WriteLine($"{(Schema.Engine.Flags)es}");
        Assert.True(isStarted);
        Assert.False(isInitialized);
        Assert.False(isOverheated);
    }
}