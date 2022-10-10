using System.Text.Json;
using DotCart.Schema;

namespace DotCart.Contract;

public interface IFeedback: IDto
{
    IEnumerable<Error> Errors { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess { get; }
    void SetError(Error error);

}





public record Feedback(string AggId, byte[] Data) 
    : Dto(AggId, Data), IFeedback
{
    public IEnumerable<Error> Errors { get; } = Array.Empty<Error>();
    public IEnumerable<string> Warnings { get; } = Array.Empty<string>();
    public IEnumerable<string> Infos { get; } = Array.Empty<string>();
    public bool IsSuccess => !Errors.Any();
    public void SetError(Error error)
    {
        Errors.Append(error);
    }


    public static IFeedback Empty => New();
    private static IFeedback New()
    {
        return new Feedback("", Array.Empty<byte>());
    }
    private static IFeedback New(string aggId)
    {
        return new Feedback(aggId, Array.Empty<byte>());
    }
    public static IFeedback New<TPayload>(string AggId, TPayload payload) where TPayload: IPayload
    {
        return new Feedback(AggId, payload.ToBytes());
    }
}