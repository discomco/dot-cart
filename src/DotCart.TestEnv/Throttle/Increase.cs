using DotCart.Schema;

namespace DotCart.TestEnv.Throttle;

public static class Increase
{
    public record Hope(string AggId, byte[] Data) : Contract.Hope(AggId, Data)
    {
        public static Hope New(string aggId, byte[] data)
        {
            return new Hope(aggId, data);
        }
    }

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta)
        {
            return new Payload(delta);
        }
    }
}