using DotCart.Abstractions.Schema;

namespace Throttle.Client;

public static class Increase
{
    public static class Topics
    {
        public const string Hope = "throttle.increase.v1";
        public const string Fact = "throttle.increased.v1";
        public const string Cmd = "throttle:increase:v1";
        public const string Evt = "throttle:increased:v1";
    }


    [HopeTopic(Topics.Hope)]
    [FactTopic(Topics.Fact)]
    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }
}