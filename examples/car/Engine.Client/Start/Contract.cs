using DotCart.Client.Contracts;
using DotCart.Core;

namespace Engine.Client.Start;

public record Payload : IPayload
{
    public static readonly Payload New = new();
}

[Topic(Topics.Fact)]
public interface IFact : DotCart.Client.Contracts.IFact
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

[Topic(Topics.Hope)]
public interface IHope : DotCart.Client.Contracts.IHope
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