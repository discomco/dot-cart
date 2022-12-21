using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Initialize
{
    public static class Topics
    {
        public const string Fact_v1 = "engine.initialized.v1";
        public const string Hope_v1 = "engine.initialize.v1";
    }


    public record Payload(Schema.Details Details) : IPayload
    {
        public Schema.Details Details { get; } = Details;

        public static Payload New(Schema.Details details)
        {
            return new Payload(details);
        }
    }

    [Topic(Topics.Hope_v1)]
    public interface IHope : IHopeT<Payload>
    {
    }

    [Topic(Topics.Hope_v1)]
    public record Hope(Payload Payload) : HopeT<Payload>(string.Empty, Payload), IHope
    {
        public static Hope New(Payload payload)
        {
            return new Hope(payload);
        }
    }

    [Topic(Topics.Fact_v1)]
    public interface IFact : IFactT<Payload>
    {
    }

    [Topic(Topics.Fact_v1)]
    public record Fact(string AggId, Payload Payload) : FactT<Payload>(AggId, Payload), IFact
    {
        public static Fact New(string AggId, Payload payload)
        {
            return new Fact(AggId, payload);
        }
    }
}