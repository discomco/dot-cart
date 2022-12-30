using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Stop
{
    public static class Topics
    {
        public const string Fact_v1 = "engine.stopped.v1";
        public const string Hope_v1 = "engine.stop.v1";
    }


    public record Payload : IPayload
    {
        public static Payload New()
        {
            return new Payload();
        }
    }

    [Topic(Topics.Fact_v1)]
    public interface IFact : IFactT<Payload>
    {
    }

    // [Topic(Topics.Fact_v1)]
    // public record Fact(string AggId, Payload Payload)
    //     : FactT<Payload>(AggId, Payload), IFact
    // {
    //     public static Fact New(string aggId, Payload payload)
    //     {
    //         return new Fact(aggId, payload);
    //     }
    // }

    [Topic(Topics.Hope_v1)]
    public interface IHope : IHopeT<Payload>
    {
    }

    [Topic(Topics.Hope_v1)]
    public record Hope(string AggId, Payload Payload)
        : HopeT<Payload>(AggId, Payload), IHope
    {
        public static Hope New(string aggId, Payload payload)
        {
            return new Hope(aggId, payload);
        }
    }
}