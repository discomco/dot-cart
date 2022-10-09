using DotCart.Schema;

namespace DotCart.Contract;

public interface IFeedback {}

public interface IFeedback<TPayload> : IFeedback
    , IDto<TPayload> where TPayload : IPayload
{
    IEnumerable<Error> Errors { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess();
}

public record Feedback<TPayload>(string AggId, byte[] Data) 
    : Dto<TPayload>(AggId, Data)
        , IFeedback<TPayload> where TPayload : IPayload
{
    public IEnumerable<Error> Errors { get; } = Array.Empty<Error>();
    public IEnumerable<string> Warnings { get; } = Array.Empty<string>();
    public IEnumerable<string> Infos { get; } = Array.Empty<string>();
    public bool IsSuccess() => !Errors.Any();
}