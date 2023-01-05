using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace DotCart.Context.Actors;

public abstract class StepT<TPayload> : IStepT<TPayload>
    where TPayload : IPayload
{
    public ISequenceT<TPayload> Sequence { get; set; }

    public string Name => GetName();

    protected abstract string GetName();

    public uint Order => GetOrder();

    private uint GetOrder()
    {
        try
        {
            return OrderAtt.Get(this);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public StepLevel Level { get; set; }

    public abstract Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback,
        CancellationToken cancellationToken = default);

    public void SetSequence(ISequenceT<TPayload> sequence)
    {
        Sequence = sequence;
    }
}