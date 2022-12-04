using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;

namespace Engine.Behavior;

public static class ChangeDetails
{
    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeDetails.Payload Payload) 
        : CmdT<Contract.ChangeDetails.Payload>(AggregateID,
        Payload)
    {
        public static Cmd New(Schema.EngineID engineId, Contract.ChangeDetails.Payload payload) 
            => new(engineId, payload);
    }
    
    
    

    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_details:v1";
        public const string Evt_v1 = "engine:details_changed:v1";
    }
    
    

    [Topic(Topics.Evt_v1)]
    public record Evt(IID AggregateID, 
        Contract.ChangeDetails.Payload Payload,
        EventMeta Meta) 
        : EvtT<Contract.ChangeDetails.Payload, EventMeta>(
            AggregateID, 
            TopicAtt.Get<Evt>(), 
            Payload, 
            Meta)
    {
        
        public static Evt New(
            Schema.EngineID engineID, 
            Contract.ChangeDetails.Payload payload)
        {
            var meta = EventMeta.New(
                nameof(Aggregate), 
                engineID.Id());
            return new Evt(engineID, payload, meta);
        }
    }
}