using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class ChangeDetails
{

    public static class Topics
    {
        public const string Hope_v1 = "engine.change_details.v1";
        public const string Fact_v1 = "engine.details_changed.v1";
    }

    public record Payload(Schema.Details Details) : IPayload
    {
        public Schema.Details Details { get; set; } = Details;

        public static Payload New(Schema.Details details)
        {
            return new Payload(details);
        }
    }
    
    [Topic(Topics.Hope_v1)]
    public record Hope(string AggId, Payload Payload) 
        : HopeT<Payload>(AggId, Payload), IHope<Payload>
    {
        public static Hope New(string aggId, Payload payload) 
            => new(aggId, payload);
    }

    [Topic(Topics.Fact_v1)]
    public record Fact(string AggId, Payload Payload) 
        : FactT<Payload>(AggId, Payload)
    {
        public static Fact New(string aggId, Payload payload) 
            => new(aggId, payload);
      
    }
}