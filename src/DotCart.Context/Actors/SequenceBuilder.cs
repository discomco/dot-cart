using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Drivers;
using DotCart.Context.Behaviors;

namespace DotCart.Context.Actors;

public class SequenceBuilder : ISequenceBuilder
{
    public SequenceBuilder(IAggregateBuilder aggBuilder, IAggregateStore aggStore)
    {
        _aggBuilder = aggBuilder;
        _aggStore = aggStore;
    }
    
    private readonly IAggregateBuilder _aggBuilder;
    private readonly IAggregateStore _aggStore;

    public ICmdHandler Build()
    {
        return new CmdHandler(_aggBuilder, _aggStore);
    }
}