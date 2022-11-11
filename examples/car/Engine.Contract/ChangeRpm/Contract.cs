using DotCart.Contract.Dtos;
using DotCart.Core;

namespace Engine.Contract.ChangeRpm;

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
    public new static Hope New(string AggId, byte[] Data)
    {
        return new Hope(AggId, Data);
    }

    public static Hope New(string AggId, Payload payload)
    {
        return new Hope(AggId, payload.ToBytes());
    }
}

[Topic(Topics.Fact)]
public interface IFact : IFact<Payload>
{
}

[Topic(Topics.Fact)]
public record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact
{
    public new static Fact New(string AggId, byte[] Data)
    {
        return new Fact(AggId, Data);
    }

    public static Fact New(string AggId, Payload payload)
    {
        return new Fact(AggId, payload.ToBytes());
    }
}