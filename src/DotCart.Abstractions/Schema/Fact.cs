namespace DotCart.Abstractions.Schema;


public delegate TMsg Fact2Msg<TPayload, out TMsg>(FactT<TPayload> fact) 
    where TPayload : IPayload; 


public interface IFactB : IDto
{
}

public interface IFactT<TPayload> : IFactB
    where TPayload : IPayload
{
}

public record FactT<TPayload>(string AggId, TPayload Payload)
    : Dto<TPayload>(AggId, Payload), IFactT<TPayload>
    where TPayload : IPayload
{
    public static FactT<TPayload> New(string aggId, TPayload payload)
    {
        return new FactT<TPayload>(aggId, payload);
    }
}