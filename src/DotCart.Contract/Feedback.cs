using DotCart.Schema;

namespace DotCart.Contract;

public interface IFeedback : IDto
{
    ErrorState ErrState { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess { get; }
    void SetError(Error error);
}

public record Feedback(string AggId, byte[] Data)
    : Dto(AggId, Data), IFeedback
{
    public static IFeedback Empty => New();
    public ErrorState ErrState { get; } = new();
    public IEnumerable<string> Warnings { get; } = Array.Empty<string>();
    public IEnumerable<string> Infos { get; } = Array.Empty<string>();
    public bool IsSuccess => ErrState.IsSuccessful;

    public void SetError(Error error)
    {
        ErrState.Errors.Add(error.Code.ToString(), error);
    }

    private static IFeedback New()
    {
        return new Feedback("", Array.Empty<byte>());
    }

    public static IFeedback New(IID ID)
    {
        return new Feedback(ID.Value, Array.Empty<byte>());
    }

    public static IFeedback New<TPayload>(string aggId, TPayload payload) where TPayload : IPayload
    {
        return new Feedback(aggId, payload.ToBytes());
    }
}