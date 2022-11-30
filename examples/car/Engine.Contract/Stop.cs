using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Stop
{
    public const string FactTopic = "engine.stopped.v1";
    public const string HopeTopic = "engine.stop.v1";

    public record Payload : IPayload
    {
        public static Payload New()
        {
            return new Payload();
        }
    }

    public interface IFact : IFact<Payload>
    {
    }

    [Topic(FactTopic)]
    public record Fact(string AggId, Payload Payload)
        : FactT<Payload>(AggId, Payload), IFact
    {
        public static Fact New(string aggId, Payload payload)
        {
            return new Fact(aggId, payload);
        }
    }

    public interface IHope : IHope<Payload>
    {
    }

    [Topic(HopeTopic)]
    public record Hope(string AggId, Payload Payload)
        : HopeT<Payload>(AggId, Payload), IHope
    {
        public static Hope New(string aggId, Payload payload)
        {
            return new Hope(aggId, payload);
        }
    }
}