using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Actors;

public class PipeT<TPipeInfo,TPayload> : IPipeT<TPipeInfo,TPayload>
    where TPayload : IPayload where TPipeInfo : IPipeInfoB
{
    private readonly IEnumerable<IStepT<TPipeInfo,TPayload>> _steps;

    private PipeT(IEnumerable<IStepT<TPipeInfo,TPayload>> steps)
    {
        _steps = steps;
    }

    public async Task<Feedback> ExecuteAsync(IDto msg, CancellationToken cancellationToken)
    {
        var fbk = Feedback.New(msg.AggId);
        try
        {
            Log.Information($"{AppVerbs.Running} pipe [{Name}]");
            if (_steps == null || !_steps.Any())
                throw new Exception($"Pipe [{Name}] has no steps.");
            foreach (var step in _steps)
            {
                fbk = await step.DoStepAsync(msg, fbk, cancellationToken);
                if (step.Level == Importance.Crucial && !fbk.IsSuccess) break;
            }

            Log.Information($"{AppFacts.Ran} pipe [{Name}]");
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

    public static IPipeT<TPipeInfo,TPayload> New<TPayload>(IEnumerable<IStepT<TPipeInfo,TPayload>> steps) where TPayload : IPayload
    {
        var seq = new PipeT<TPipeInfo,TPayload>(steps);
        foreach (var step in steps) step.SetPipe(seq);
        return seq;
    }
}