using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class ChangeRpm
{
    public static class Topics
    {
        public const string Hope = "engine.change_rpm.v1";
        public const string Fact = "engine.rpm_changed.v1";
    }

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }

    [Topic(Topics.Hope)]
    public interface IHope : IHope<Payload>
    {
    }

    [Topic(Topics.Hope)]
    public record Hope(string AggId, Payload Payload) : HopeT<Payload>(AggId, Payload), IHope
    {
//        public Payload Payload { get; set; } = Payload;
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