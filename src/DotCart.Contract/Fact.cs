using DotCart.Schema;

namespace DotCart.Contract;

public interface IFact : IDto {}

public interface IFact<TPayload>: 
    IFact, 
    IDto<TPayload> where TPayload : IPayload {}

public abstract record Fact<TPayload>(string AggId, byte[] Data) : 
    Dto<TPayload>(AggId, Data),  
    IFact<TPayload> where TPayload : IPayload;