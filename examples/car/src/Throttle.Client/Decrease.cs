using DotCart.Abstractions.Schema;

namespace Throttle.Client;

public static class Decrease
{
    public static class Topics
    {
        public const string Hope = "throttle.decrease.v1";
        public const string Fact = "throttle.decreased.v1";
    }


    [Topic(Topics.Hope)]
    public interface IHope : IHopeT<Payload>
    {
    }

    [Topic(Topics.Fact)]
    public interface IFact : IFactT<Payload>
    {
    }

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }
}