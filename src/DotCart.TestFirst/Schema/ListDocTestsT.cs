using System.Text.Json;
using DotCart.Abstractions.Schema;
using DotCart.TestKit;
using Xunit.Abstractions;

namespace DotCart.TestFirst.Schema;

public abstract class ListDocTestsT<TID, TListState, TItem>
    : IoCTests
    where TID : IID
    where TListState : IListState
    where TItem : IValueObject
{
    protected ListDocTestsT(ITestOutputHelper output, IoCTestContainer testEnv)
        : base(output, testEnv)
    {
    }

    private static string serializedState;

    protected IDCtorT<TID> NewId;
    protected StateCtorT<TListState> NewState;


    [Fact]
    public void ShouldResolveIDCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var newID = TestEnv.ResolveRequired<IDCtorT<TID>>();
        // THEN
        Assert.NotNull(newID);
    }

    [Fact]
    public void ShouldResolveStateCtor()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var ctor = TestEnv.ResolveRequired<StateCtorT<TListState>>();
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
        var newState = TestEnv.Resolve<StateCtorT<TListState>>();
        // WHEN
        var state = newState();
        // THEN
        Assert.NotNull(state);
    }

    [Fact]
    public void ShouldCreateSameStates()
    {
        // GIVEN
        var newState = TestEnv.Resolve<StateCtorT<TListState>>();
        // WHEN
        var state1 = newState();
        var state2 = newState();
        // THEN
        Assert.Equal(state1, state2);
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
        var eng = JsonSerializer.Deserialize<TListState>(serializedState);
        // THEN
        Assert.NotNull(eng);
    }

    protected override void Initialize()
    {
        NewId = TestEnv.ResolveRequired<IDCtorT<TID>>();
        NewState = TestEnv.ResolveRequired<StateCtorT<TListState>>();
    }
}