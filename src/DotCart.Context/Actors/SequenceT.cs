using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Actors;

public class SequenceT<TPayload> : ISequenceT<TPayload>
    where TPayload : IPayload
{
    private readonly IEnumerable<IStepT<TPayload>> _steps;

    private SequenceT(IEnumerable<IStepT<TPayload>> steps)
    {
        _steps = steps;
    }

    public async Task<Feedback> ExecuteAsync(IDto msg, CancellationToken cancellationToken)
    {
        var fbk = Feedback.New(msg.AggId);
        try
        {
            Log.Information($"{AppVerbs.Running} sequence [{Name}]");
            if (_steps==null || !_steps.Any()) 
                throw new Exception($"Sequence [{Name}] has no steps.");
            foreach (var step in _steps)
            {
                fbk = await step.ExecuteAsync(msg, fbk, cancellationToken);
                if (step.Level == StepLevel.Crucial && !fbk.IsSuccess) break;
            }
            Log.Information($"{AppFacts.Ran} sequence [{Name}]");
        }
        catch (Exception e)
        {
            Log.Error($"{AppErrors.Error(e.InnerAndOuter())}");
            fbk.SetError(e.AsError());
        }

        return fbk;
    }

    public string Name => $"{typeof(TPayload).FullName}:seq";

    public int StepCount => _steps.Count();

    public static ISequenceT<TPayload> New<TPayload>(IEnumerable<IStepT<TPayload>> steps) where TPayload : IPayload
    {
        var seq = new SequenceT<TPayload>(steps);
        foreach (var step in steps)
        {
            step.SetSequence(seq);
        }
        return seq;
    }
}