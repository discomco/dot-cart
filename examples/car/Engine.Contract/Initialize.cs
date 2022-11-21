using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Initialize
{
    public static class Topics
    {
        public const string Fact = "engine.initialized.v1";
        public const string Hope = "engine.initialize.v1";
    }

    public record Payload : IPayload
    {
        public Payload()
        {
        }

        private Payload(Schema.Details details)
        {
            Details = details;
        }

        public Schema.Details Details { get; }

        public static Payload New(Schema.Details details)
        {
            return new Payload(details);
        }
    }

    [Topic(Topics.Hope)]
    public interface IHope : IHope<Payload>
    {
    }

    [Topic(Topics.Hope)]
    public record Hope(string AggId, Payload Payload) : HopeT<Payload>(AggId, Payload), IHope
    {
        public static Hope New(string AggId, byte[] Data)
        {
            return new Hope(AggId, Data.FromBytes<Payload>());
        }

        public static Hope New(string AggId, Payload payload)
        {
            return new Hope(AggId, payload);
        }
    }

    [Topic(Topics.Fact)]
    public interface IFact : IFact<Payload>
    {
    }

    [Topic(Topics.Fact)]
    public record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact
    {
        public static Fact New(string AggId, byte[] Data)
        {
            return new Fact(AggId, Data);
        }

        public static Fact New(string AggId, Payload payload)
        {
            return new Fact(AggId, payload.ToBytes());
        }
    }
}