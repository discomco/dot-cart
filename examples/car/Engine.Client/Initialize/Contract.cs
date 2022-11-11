using DotCart.Client.Contracts;
using DotCart.Core;
using Engine.Client.Schema;

namespace Engine.Client.Initialize;

public record Payload : IPayload
{
    public Payload()
    {
    }

    private Payload(Details details)
    {
        Details = details;
    }

    public Details Details { get; }

    public static Payload New(Details details)
    {
        return new Payload(details);
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