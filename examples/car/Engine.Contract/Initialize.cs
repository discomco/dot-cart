using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Initialize
{
    public const string FactTopic = "engine.initialized.v1";
    public const string HopeTopic = "engine.initialize.v1";

    public record Payload(Schema.Details Details) : IPayload
    {
        public Schema.Details Details { get; } = Details;

        public static Payload New(Schema.Details details)
        {
            return new Payload(details);
        }
    }

    [Topic(HopeTopic)]
    public interface IHope : IHope<Payload>
    {
    }

    [Topic(HopeTopic)]
    public record Hope(Payload Payload) : HopeT<Payload>(string.Empty, Payload), IHope
    {
        public static Hope New(Payload payload)
        {
            return new Hope(payload);
        }
    }

    [Topic(FactTopic)]
    public interface IFact : IFact<Payload>
    { }

    [Topic(FactTopic)]
    public record Fact(string AggId, Payload Payload) : FactT<Payload>(AggId, Payload), IFact
    {
        public static Fact New(string AggId, Payload payload)
        {
            return new Fact(AggId, payload);
        }
    }
}