using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behavior;

public class ChoreographyT<TCmdPayload, TEvtPayload, TMeta> : IChoreography
    where TCmdPayload : IPayload
    where TMeta : IMeta
{
    private readonly Evt2Cmd<TCmdPayload, TEvtPayload, TMeta> _evt2Cmd;
    private IAggregate _aggregate;


    public ChoreographyT(Evt2Cmd<TCmdPayload, TEvtPayload, TMeta> evt2Cmd)
    {
        _evt2Cmd = evt2Cmd;
    }

    public IChoreography SetAggregate(IAggregate aggregate)
    {
        _aggregate = aggregate;
        return this;
    }

    public string
        Name => NameAtt.ChoreographyName<TEvtPayload, TCmdPayload>();

    public string
        Topic => $"{EvtTopicAtt.Get<TEvtPayload>()}";


    public async Task<Feedback> WhenAsync(IEvtB evt)
    {
        var feedback = Feedback.New(evt.AggregateId);
        try
        {
            Log.Information($"{AppVerbs.Enforcing} [{Name}]");
            var cmd = _evt2Cmd((Event)evt, _aggregate.GetState());
            if (cmd != null)
                feedback = await _aggregate.ExecuteAsync(cmd);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return feedback;
    }
}