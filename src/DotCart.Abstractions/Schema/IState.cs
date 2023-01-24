namespace DotCart.Abstractions.Schema;

public interface IState : IPayload
{
    string Id { get; }
    string? Rev { get; set; }
}