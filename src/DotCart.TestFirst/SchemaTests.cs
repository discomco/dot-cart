using System.Text.Json;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class SchemaTests<TID, TState> : IoCTests
    where TID : IID
    where TState : IState
{
    private static string serializedState;

    protected IDCtor<TID> NewId;
    protected StateCtor<TState> NewState;

    protected SchemaTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
    {
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtor<TID>>();
        // THEN
        Assert.NotNull(newID);
    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ctor = TestEnv.ResolveRequired<StateCtor<TState>>();
        // THEN
        Assert.NotNull(ctor);
    }

    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        Assert.NotNull(NewId);
        // WHEN

        var ID = NewId();
        // THEN
        Assert.NotNull(ID);
    }


    [Fact]
    public void ShouldCreateState()
    {
        // GIVEN
        var newState = TestEnv.Resolve<StateCtor<TState>>();
        // WHEN
        var state = newState();
        // THEN
        Assert.NotNull(state);
    }

    [Fact]
    public void ShouldCreateDifferentStates()
    {
        // GIVEN
        var newState = TestEnv.Resolve<StateCtor<TState>>();
        // WHEN
        var state1 = newState();
        var state2 = newState();
        // THEN
        Assert.NotSame(state1, state2);
    }

    [Fact]
    public void ShouldBeSerializable()
    {
        // GIVEN
        Assert.NotNull(NewState);
        var state = NewState();
        // WHEN
        serializedState = JsonSerializer.Serialize(state);
        // THEN
        Assert.NotEmpty(serializedState);
    }

    [Fact]
    public void ShouldStateBeDeserializable()
    {
        // GIVEN
        ShouldBeSerializable();
        // WHEN
        var eng = JsonSerializer.Deserialize<TState>(serializedState);
        // THEN
        Assert.NotNull(eng);
    }


    protected override void Initialize()
    {
        NewId = TestEnv.ResolveRequired<IDCtor<TID>>();
        NewState = TestEnv.ResolveRequired<StateCtor<TState>>();
    }
}