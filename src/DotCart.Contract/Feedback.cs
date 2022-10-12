using System.Text.Json;
using DotCart.Schema;

namespace DotCart.Contract;

public interface IFeedback: IDto
{
    ICollection<Error> Errors { get; }
    IEnumerable<string> Warnings { get; }
    IEnumerable<string> Infos { get; }
    bool IsSuccess { get; }
    void SetError(Error error);

}





public record Feedback(string AggId, byte[] Data) 
    : Dto(AggId, Data), IFeedback
{
    public ICollection<Error> Errors { get; } = new List<Error>();
    public IEnumerable<string> Warnings { get; } = Array.Empty<string>();
    public IEnumerable<string> Infos { get; } = Array.Empty<string>();
    public bool IsSuccess => !Errors.Any();
    public void SetError(Error error)
    {
        Errors.Add(error);
    }


    public static IFeedback Empty => New();
    private static IFeedback New()
    {
        return new Feedback("", Array.Empty<byte>());
    }

    public static IFeedback New(IID ID)
    {
        return new Feedback(ID.Value, Array.Empty<byte>());
    }
    public static IFeedback New<TPayload>(string aggId, TPayload payload) where TPayload: IPld
    {
        return new Feedback(aggId, payload.ToBytes());
    }
}