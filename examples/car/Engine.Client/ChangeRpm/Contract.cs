using DotCart.Client.Contracts;
using DotCart.Core;

namespace Engine.Client.ChangeRpm;

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
public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
{
    public static Hope New(string AggId, byte[] Data)
    {
        return new(AggId, Data);
    }

    public static Hope New(string AggId, Payload payload)
    {
        return new(AggId, payload.ToBytes());
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
        return new(AggId, Data);
    }

    public static Fact New(string AggId, Payload payload)
    {
        return new(AggId, payload.ToBytes());
    }
}