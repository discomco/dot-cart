namespace DotCart.Abstractions.Schema;

public interface IFactB : IDto
{
}

public interface IFactT<TPayload> : IFactB
    where TPayload : IPayload
{
}

public record FactT<TPayload, TMeta>(string AggId, TPayload Payload, TMeta Meta)
    : Dto<TPayload>(AggId, Payload), IFactT<TPayload>
    where TPayload : IPayload
{
    public TMeta Meta { get; set; } = Meta;

    public static FactT<TPayload, TMeta> New(string aggId, TPayload payload, TMeta meta)
    {
        return new FactT<TPayload, TMeta>(aggId, payload, meta);
    }
}