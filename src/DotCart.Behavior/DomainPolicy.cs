using DotCart.Contract;
using Serilog;

namespace DotCart.Behavior;

public delegate TCmd Evt2Cmd<in TEvt, out TCmd>(TEvt Evt) where TEvt : IEvt where TCmd : ICmd;

public abstract class DomainPolicy<TEvt, TCmd> : IDomainPolicy where TEvt : IEvt where TCmd : ICmd
{
    private readonly Evt2Cmd<TEvt, TCmd> _evt2Cmd;
    protected IAggregate? Aggregate;

    protected DomainPolicy
    (
        ITopicMediator mediator,
        Evt2Cmd<TEvt, TCmd> evt2Cmd
    )
    {
        _evt2Cmd = evt2Cmd;
        mediator.Subscribe(Topic.Get<TEvt>(), HandleEvt);
    }


    public void SetBehavior(IAggregate aggregate)
    {
        Aggregate = aggregate;
    }

    private Task HandleEvt(IEvt arg)
    {
        return Task.Run(async () =>
        {
            var fbk = await Enforce((TEvt)arg);
            if (!fbk.IsSuccess) Log.Error(fbk.ErrState.ToString());
        });
    }

    private Task<IFeedback> Enforce(TEvt evt)
    {
        var cmd = _evt2Cmd(evt);
        return Aggregate.ExecuteAsync(cmd);
    }
}