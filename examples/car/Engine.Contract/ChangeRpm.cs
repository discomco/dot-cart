using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class ChangeRpm
{
    public static class Topics
    {
        public const string Hope_v1 = "engine.change_rpm.v1";
        public const string Fact_v1 = "engine.rpm_changed.v1";
    }

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }

    [Topic(Topics.Hope_v1)]
    public interface IHope : IHopeT<Payload>
    {
    }

    [Topic(Topics.Hope_v1)]
    public record Hope(string AggId, Payload Payload) : HopeT<Payload>(AggId, Payload), IHope
    {
//        public Payload Payload { get; set; } = Payload;
        public static Hope New(string AggId, Payload payload)
        {
            return new Hope(AggId, payload);
        }
    }

    [Topic(Topics.Fact_v1)]
    public interface IFact : IFactT<Payload>
    {
    }

    [Topic(Topics.Fact_v1)]
    public record Fact(string AggId, Payload Payload)
        : FactT<Payload>(AggId, Payload), IFact
    {
        public static Fact New(string AggId, Payload payload)
        {
            return new Fact(AggId, payload);
        }
    }
}