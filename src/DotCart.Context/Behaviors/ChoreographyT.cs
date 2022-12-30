using DotCart.Abstractions;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Behaviors;

public class ChoreographyT<TEvt, TCmd> : IChoreography
    where TEvt : IEvtB
    where TCmd : ICmdB
{
    private readonly Evt2Cmd<TCmd, TEvt> _evt2Cmd;
    private IAggregate _aggregate;


    public ChoreographyT(Evt2Cmd<TCmd, TEvt> evt2Cmd)
    {
        _evt2Cmd = evt2Cmd;
    }

    public void SetAggregate(IAggregate aggregate)
    {
        _aggregate = aggregate;
    }

    public string Name => NameAtt.ChoreographyName<TEvt, TCmd>();
    public string Topic => $"{TopicAtt.Get<TEvt>()}";


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