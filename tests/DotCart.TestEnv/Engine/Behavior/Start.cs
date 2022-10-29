using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Drivers.Ardalis;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Behavior;


public partial class EngineAggregate :
    IExec<Start.Cmd>,
    IApply<Start.Evt>
{
    public IFeedback Verify(Start.Cmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            DotGuard.Against.StateIsNotInitialized(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }
        return fbk;
    }

    public IEnumerable<IEvt> Exec(Start.Cmd cmd)
    {
        return new[]
        {
            new Start.Evt(cmd.AggregateID, cmd.Payload)
        };
    }

    public void Apply(Start.Evt evt)
    {
        _state.Status = (EngineStatus)((int)_state.Status).SetFlag((int)EngineStatus.Started);
    }
}

public static class Start
{

    public const string CmdTopic = "engine:start:v1";
    public const string EvtTopic = "engine:started:v1";

    public class StartOnInitializedPolicy : DomainPolicy<IEngineAggregate, Initialize.IEvt>, IEnginePolicy
    {
        public StartOnInitializedPolicy(ITopicPubSub pubSub) : base(Initialize.EvtTopic, pubSub)
        {
        }

        protected override async Task Enforce(DotCart.Behavior.IEvt evt)
        {
            var cmd = Cmd.New(evt.AggregateID, Payload.New);
            var fbk = await Aggregate.ExecuteAsync(cmd);
            Console.WriteLine(fbk.GetPayload<Schema.Engine>());
        }
    }
    
    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }
    
    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddTransient<IEnginePolicy, StartOnInitializedPolicy>();
    }
    
    public static void StateIsNotInitialized(this IClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)EngineStatus.Initialized))
            throw new NotInitializedException($"engine {state.Id}  is not initialized");
    }

    
    public interface ICmd : ICmd<Payload>
    {
    }


    public record Cmd
        (IID AggregateID, Payload Payload) :
            Cmd<Payload>(CmdTopic, AggregateID, Payload), ICmd
    {
        public static Cmd New(IID aggID, Payload payload)
        {
            return new Cmd(aggID, payload);
        }
    }
    
    public interface IEvt : IEvt<Payload>
    {
    }
    
    public record Evt
        (IID AggregateID, Payload Payload) :
            Evt<Payload>(EvtTopic, AggregateID, Payload), IEvt;
    
}