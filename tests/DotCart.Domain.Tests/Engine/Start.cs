using Ardalis.GuardClauses;
using DotCart.Contract;
using DotCart.Schema;
using DotCart.Schema.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.Domain.Tests.Engine;


public partial class EngineAggregate :
    IExec<Schema.Tests.Engine, Start.Cmd>,
    IApply<Schema.Tests.Engine, Start.Evt>
{
    public IFeedback Verify(Schema.Tests.Engine state, Start.Cmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.StateIsNotInitialized(state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsApiError());
        }
        return fbk;
    }

    public IEnumerable<Domain.IEvt> Exec(Schema.Tests.Engine state, Start.Cmd cmd)
    {
        return new[]
        {
            new Start.Evt(cmd.AggregateID, cmd.Pload)
        };
    }

    public Schema.Tests.Engine Apply(Schema.Tests.Engine state, Start.Evt evt)
    {
        state.Status = (EngineStatus)((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
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
            var cmd = Cmd.New(evt.AggregateID, Pld.New);
            var fbk = await Aggregate.ExecuteAsync(cmd);
            Console.WriteLine(fbk.GetPayload<Schema.Tests.Engine>());
        }
    }
    
    public record Pld : IPld
    {
        public static readonly Pld New = new();
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

    
    public interface ICmd : ICmd<Pld>
    {
    }


    public record Cmd
        (IID AggregateID, Pld Pld) :
            Cmd<Pld>(CmdTopic, AggregateID, Pld), ICmd
    {
        public static Cmd New(IID aggID, Pld pld)
        {
            return new Cmd(aggID, pld);
        }
    }
    
    public interface IEvt : IEvt<Pld>
    {
    }
    public record Evt
        (IID AggregateID, Pld Pld) :
            Evt<Pld>(EvtTopic, AggregateID, Pld), IEvt;
    
}