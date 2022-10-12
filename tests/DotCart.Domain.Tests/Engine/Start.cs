using Ardalis.GuardClauses;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.Schema.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Domain.Tests.Engine;


public partial class EngineAggregate :
    IExec<Start.Cmd>,
    IApply<Start.Evt>
{
    public IFeedback Verify(Start.Cmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.StateIsNotInitialized(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsApiError());
        }
        return fbk;
    }

    public IEnumerable<IEvt> Exec(Start.Cmd cmd)
    {
        return new[]
        {
            new Start.Evt(cmd.AggregateID, cmd.Pload)
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

    public class StartOnInitializedPolicy : AggregatePolicy<IEngineAggregate, Initialize.IEvt>, IEnginePolicy
    {
        public StartOnInitializedPolicy(ITopicPubSub pubSub) : base(Initialize.EvtTopic, pubSub)
        {
        }

        protected override async Task Enforce(Domain.IEvt evt)
        {
            var cmd = Cmd.New(evt.AggregateID, Payload.New);
            var fbk = await Aggregate.ExecuteAsync(cmd);
            Console.WriteLine(fbk.GetPayload<Schema.Tests.Engine>());
        }
    }
    
    public record Payload : IPld
    {
        public static readonly Payload New = new();
    }
    
    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddTransient<IEnginePolicy, StartOnInitializedPolicy>();
    }
    
    public static void StateIsNotInitialized(this IGuardClause guard, Schema.Tests.Engine state)
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