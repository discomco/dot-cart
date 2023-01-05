using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DotCart.Context.Actors;


public static partial class Inject
{
    public static IServiceCollection AddHopeSequence<THopePayload, TMeta>(this IServiceCollection services) 
        where THopePayload : IPayload where TMeta : IEventMeta
    {
        return services
            .AddTransient<IStepT<THopePayload>, CmdHandlerStepT<THopePayload,TMeta>>()
            .AddSequenceBuilder<THopePayload>();
    }
   
}

[Name("CmdHandler")]
public class CmdHandlerStepT<TPayload, TMeta> : StepT<TPayload>
    where TPayload : IPayload
    where TMeta : IEventMeta
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TPayload, TMeta> _hope2Cmd;

    public CmdHandlerStepT(Hope2Cmd<TPayload, TMeta> hope2Cmd, ICmdHandler cmdHandler)
    {
        Level = StepLevel.Crucial;
        _hope2Cmd = hope2Cmd;
        _cmdHandler = cmdHandler;
    }

    protected override string GetName()
    {
        return NameAtt.StepName(Level, NameAtt.Get(this), FactTopicAtt.Get<TPayload>());
    }

    public override async Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback,
        CancellationToken cancellationToken = default)
    {
        var feedback = Feedback.New(msg.AggId, previousFeedback, Name);
        try
        {
            Log.Information($"{AppVerbs.Executing} {Name}");
            var cmd = _hope2Cmd((HopeT<TPayload>)msg);
            feedback = await _cmdHandler.HandleAsync(cmd, previousFeedback, cancellationToken);
            Log.Information($"{AppFacts.Executed} {Name}");
        }
        catch (Exception e)
        {
            Log.Error($"{AppErrors.Error(e.InnerAndOuter())} ");
            feedback.SetError(e.AsError());
        }

        return feedback;
    }
}