using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Start
{
    public static class Topics
    {
        public const string Hope_v1 = "engine.start.v1";
        public const string Fact_v1 = "engine.started.v1";
    }

    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }

    [Topic(Topics.Fact_v1)]
    public interface IFact : IFactT<Payload>
    {
    }

    // [Topic(Topics.Fact_v1)]
    // public record Fact(string AggId, Payload Payload) : FactT<Payload>(AggId, Payload), IFact
    // {
    //     public static Fact New(string AggId, Payload payload)
    //     {
    //         return new Fact(AggId, payload);
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
        public static Hope New(string AggId, Payload payload)
        {
            return new Hope(AggId, payload);
        }
    }
}