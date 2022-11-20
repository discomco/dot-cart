using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Behavior;

public delegate IAggregate AggCtor();

public delegate IAggregate AggBuilder(AggCtor newAgg);

public interface IAggregate
{
    IID ID { get; }
    bool IsNew { get; }
    long Version { get; }
    IEnumerable<IEvt> UncommittedEvents { get; }
    bool KnowsTry(string cmdType);
    bool KnowsApply(string evtType);
    void InjectTryFuncs(IEnumerable<ITry> tryFuncs);
    void InjectApplyFuncs(IEnumerable<IApply> applyFuncs);
    Task<Feedback> ExecuteAsync(ICmd cmd);
    IAggregate SetID(IID ID);
    string Id();
    string GetName();
    void InjectPolicies(IEnumerable<IAggregatePolicy> policies);
    void Load(IEnumerable<IEvt>? events);
    void ClearUncommittedEvents();
    IState GetState();
    EventMeta GetMeta();
    void ClearUncommittedEvents(ulong resNextExpectedVersion);
}