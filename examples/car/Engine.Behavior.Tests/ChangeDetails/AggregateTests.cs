using DotCart.TestFirst;
using DotCart.TestFirst.Behavior;
using Engine.Contract;

namespace Engine.Behavior.Tests.ChangeDetails;

public class AggregateTests : AggregateTestsT<
    Schema.EngineID, 
    Engine, 
    Behavior.ChangeDetails.TryCmd,
    Behavior.ChangeDetails.ApplyEvt, 
    Behavior.ChangeDetails.Cmd, 
    Behavior.ChangeDetails.IEvt>
{
    
}