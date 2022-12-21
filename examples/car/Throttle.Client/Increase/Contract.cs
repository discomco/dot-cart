using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Throttle.Client.Increase;

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