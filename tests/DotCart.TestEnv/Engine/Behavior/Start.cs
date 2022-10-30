using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
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
    public const string CmdTopic = "engine:start:v1";
    public const string EvtTopic = "engine:started:v1";

    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddTransient<IDomainPolicy, StartOnInitializedPolicy>();
    }

    public static void StateIsNotInitialized(this IGuardClause guard, Schema.Engine state)
    {
        if (((int)state.Status).NotHasFlag((int)EngineStatus.Initialized))
            throw new NotInitializedException($"engine {state.Id}  is not initialized");
    }

    public class StartOnInitializedPolicy : DomainPolicy<Initialize.IEvt>
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
            Evt<Payload>(EvtTopic, AggregateID, Payload), IEvt
    {
        public static Evt New(IID engineID, Payload payload)
        {
            return new(engineID, payload);
        }
    }
}