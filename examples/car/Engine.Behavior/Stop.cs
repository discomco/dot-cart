using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Engine.Contract;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Engine.Behavior;

public static class Stop
{
    
    
    public static IServiceCollection AddStopBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineContract()
            .AddBaseBehavior()
            .AddStopMappers()
            .AddTransient<ITry, TryCmd>()
            .AddTransient<IApply, ApplyEvt>();
    }

    public static IServiceCollection AddStopMappers(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _hope2Cmd);
    }

    public static Hope2Cmd<Stop.Cmd, Contract.Stop.Hope> _hope2Cmd = hope =>
        Stop.Cmd.New(hope.AggId.IDFromIdString(), hope.Payload);





    public const string EvtTopic = "engine:stopped:v1";
    public const string CmdTopic = "engine:stop:v1";

    [Topic(CmdTopic)]
    public record Cmd(string Topic, IID AggregateID, Contract.Stop.Payload Payload)
        : CmdT<Contract.Stop.Payload>(Topic, AggregateID, Payload)
    {
        public static Cmd New(ID ID, Contract.Stop.Payload payload)
        {
            return new Cmd(CmdTopic, ID, payload);
        }
    }

    [Topic(EvtTopic)]
    public interface IEvt : IEvtT<Contract.Stop.Payload>
    {
    }
    public class TryCmd : TryCmdT<Cmd>
    {
        public override IFeedback Verify(Cmd cmd)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineNotStarted((Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                Log.Debug(e.InnerAndOuter());
                fbk.SetError(e.AsError());
            }

            return fbk;
        }

        public override IEnumerable<Event> Raise(Cmd cmd)
        {
            return new[]
            {
                Event.New(
                    (Schema.EngineID)cmd.AggregateID,
                    TopicAtt.Get<IEvt>(),
                    cmd.Payload,
                    Aggregate.GetMeta(),
                    Aggregate.Version
                    )
            };
        }
    }

    public class ApplyEvt : ApplyEvtT<Engine, IEvt>
    {
        public override Engine Apply(Engine state, Event evt)
        {
            state.Status = (Schema.EngineStatus)((int)state.Status).UnsetFlag((int)Schema.EngineStatus.Started);
            state.Status = (Schema.EngineStatus)((int)state.Status).SetFlag((int)Schema.EngineStatus.Stopped);
            state.Power = 0;
            return state;
        }
    }
}