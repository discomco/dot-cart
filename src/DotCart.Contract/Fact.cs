namespace DotCart.Contract;

public interface IFact : IDto {}

public abstract record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact; 