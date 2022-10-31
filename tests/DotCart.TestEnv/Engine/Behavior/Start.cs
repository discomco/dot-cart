using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine.Behavior;

public partial class EngineAggregate :
    ITry<Start.Cmd>,
    IApply<Schema.Engine, Start.Evt>
{
    public IState Apply(Schema.Engine state, Start.Evt evt)
    {
        state.Status = (EngineStatus)((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    }

    public IFeedback Verify(Start.Cmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.StateIsNotInitialized(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return fbk;
    }

    public IEnumerable<IEvt> Raise(Start.Cmd cmd)
    {
        return new[]
        {
            Start.Evt.New(cmd.AggregateID, cmd.Payload)
        };
    }
}

public static class Start
{
    private const string CmdTopic = "engine:start:v1";
    private const string EvtTopic = "engine:started:v1";

    private static Evt2Cmd<Initialize.Evt, Cmd> evt2Cmd => evt => Cmd.New(evt.AggregateID, Payload.New); 

    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddTransient(_ => evt2Cmd)
            .AddTransient<IDomainPolicy, StartOnInitializedPolicy>();
    }


    public static void StateIsNotInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)EngineStatus.Initialized))
            throw new NotInitializedException($"engine {state.Id}  is not initialized");
    }

    public class StartOnInitializedPolicy : DomainPolicy<Initialize.Evt, Cmd>
    {

        // protected override async Task Enforce(DotCart.Behavior.IEvt evt)
        // {
        //     var cmd = Cmd.New(evt.AggregateID, Payload.New);
        //     var fbk = await Aggregate.ExecuteAsync(cmd);
        //     Console.WriteLine(fbk.GetPayload<Schema.Engine>());
        // }

        public StartOnInitializedPolicy(
            ITopicMediator mediator, 
            Evt2Cmd<Initialize.Evt, Cmd> evt2Cmd) 
            : base(mediator, evt2Cmd)
        {
        }
    }

    public record Payload : IPayload
    {
        public static readonly Payload New = new();
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

    [Topic(EvtTopic)]
    public record Evt
        (IID AggregateID, Payload Payload) :
            Evt<Payload>(EvtTopic, AggregateID, Payload), IEvt
    {
        public static Evt New(IID engineID, Payload payload)
        {
            return new Evt(engineID, payload);
        }
    }
}