using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Contract;

public interface IFeedback : IDto
{
    ErrorState ErrState { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess { get; }
    IFeedback? Previous { get; }
    IPayload Payload { get; }
    string Step { get; }
    void SetError(Error error);

    void SetPayload<TPayload>(TPayload state)
        where TPayload : IPayload;

    TPayload GetPayload<TPayload>()
        where TPayload : IPayload;

    void AddWarning(string warning);
    void AddInfo(string info);
}