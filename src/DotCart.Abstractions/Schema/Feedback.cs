namespace DotCart.Abstractions.Schema;

public interface IFeedback : IDto, IMsg
{
    ErrorState ErrState { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess { get; }
    void SetError(Error error);
    void SetPayload<TState>(TState state) where TState : IState;
}

public record Feedback(string AggId) : IFeedback
{
    public static Feedback Empty => New("");
    public IState Payload { get; set; }
    public ErrorState ErrState { get; } = new();
    public IEnumerable<string> Warnings { get; } = Array.Empty<string>();
    public IEnumerable<string> Infos { get; } = Array.Empty<string>();
    public bool IsSuccess => ErrState.IsSuccessful;

    public string AggId { get; set; } = AggId;

    public void SetError(Error error)
    {
        ErrState.Errors.Add(error.Code.ToString(), error);
    }

    public void SetPayload<TState>(TState state) where TState : IState
    {
        Payload = state;
    }

    public static Feedback New(string Id)
    {
        return new Feedback(Id);
    }

    public static Feedback New<TPayload>(string aggId, TPayload payload) where TPayload : IPayload
    {
        return new Feedback(aggId);
    }
}