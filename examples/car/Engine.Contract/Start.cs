using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Start
{
    public static class Topics
    {
        public const string Hope = "engine.start.v1";
        public const string Fact = "engine.started.v1";
    }

    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }

    [Topic(Topics.Fact)]
    public interface IFact : IFact<Payload>
    {
    }

    [Topic(Topics.Fact)]
    public record Fact(string AggId, Payload Payload) : FactT<Payload>(AggId, Payload), IFact
    {
        public static Fact New(string AggId, Payload payload)
        {
            return new Fact(AggId, payload);
        }
    }

    [Topic(Topics.Hope)]
    public interface IHope : IHope<Payload>
    {
    }

    [Topic(Topics.Hope)]
    public record Hope(string AggId, Payload Payload)
        : HopeT<Payload>(AggId, Payload), IHope
    {
        public static Hope New(string AggId, Payload payload)
        {
            return new Hope(AggId, payload);
        }
    }
}