using System.Text.Json;
using DotCart.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst;

public abstract class SchemaTests<TID, TState>: IoCTests
  where TID : IID
  where TState : IState
{

    protected NewID<TID> NewID;
    protected NewState<TState> NewState;

    protected SchemaTests(ITestOutputHelper output, IoCTestContainer container) : base(output, container)
    {
    }


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var newID = Container.GetRequiredService<NewID<TID>>();
        // THEN
        Assert.NotNull(newID);

    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(Container);
        // WHEN
        var ctor = Container.GetRequiredService<NewState<TState>>();
        // THEN
        Assert.NotNull(ctor);
    }

    [Fact]
    public void ShouldCreateID()
    {
        // GIVEN
        Assert.NotNull(Container);
        Assert.NotNull(NewID);
        // WHEN
        
        var ID = NewID();
        // THEN
        Assert.NotNull(ID);
    }
    
    
    
    [Fact]
    public void ShouldCreateState()
    {
        // GIVEN
        var newState = Container.GetService<NewState<TState>>();
        // WHEN
        var state = newState();
        // THEN
        Assert.NotNull(state);
    }
    [Fact]
    public void ShouldCreateDifferentStates()
    {
        // GIVEN
        var newState = Container.GetService<NewState<TState>>();
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

    private static string serializedState;
    
    [Fact]
    public  void ShouldStateBeDeserializable()
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
        NewID = Container.GetRequiredService<NewID<TID>>();
        NewState = Container.GetRequiredService<NewState<TState>>();
    }
}