namespace DotCart.Abstractions.Schema;

public interface IFeedback : IDto
{
    ErrorState ErrState { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess { get; }
    IFeedback? Previous { get; }
    string Step { get; }
    void SetError(Error error);
    void SetPayload<TPayload>(TPayload state) 
        where TPayload : IPayload;

    TPayload GetPayload<TPayload>() 
        where TPayload : IPayload;
    void AddWarning(string warning);
    void AddInfo(string info);
}

public record Feedback(string AggId, IFeedback Previous = null, string Step = "") : IFeedback
{
    public static Feedback Empty => New("");
    public IPayload Payload { get; private set; }
    public ErrorState ErrState { get; } = new();
    public IEnumerable<string> Warnings { get; private set; } = Array.Empty<string>();
    public IEnumerable<string> Infos { get; private set; } = Array.Empty<string>();
    public bool IsSuccess => ErrState.IsSuccessful;

    public string AggId { get; set; } = AggId;
    public string Step { get; } = Step;

    public void SetError(Error error)
    {
        ErrState.Errors.Add(error.Code.ToString(), error);
    }

    public void SetPayload<TPayload>(TPayload state) 
        where TPayload : IPayload
    {
        Payload = state;
    }

    public TPayload GetPayload<TPayload>() 
        where TPayload : IPayload
    {
        return (TPayload)Payload;
    }

    public IFeedback? Previous { get; } = Previous;

    public void AddWarning(string warning)
    {
        Warnings = Warnings.Append(warning);
    }

    public void AddInfo(string info)
    {
        Infos = Infos.Append(info);
    }

    public static Feedback New(string Id, IFeedback? previous = null, string step = "")
    {
        return new Feedback(Id, previous, step);
    }
}