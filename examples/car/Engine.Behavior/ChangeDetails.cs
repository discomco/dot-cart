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


public static partial class Inject
{
    public static IServiceCollection AddChangeDetailsBehavior(this IServiceCollection services)
    {
        return services
            .AddTransient<ITry<ChangeDetails.Cmd, Engine>, ChangeDetails.TryCmd>();
    }
} 



public static class ChangeDetails
{
    public class TryCmd : TryCmdT<Cmd, Engine>
    {
        public override IFeedback Verify(Cmd cmd, Engine state)
        {
            var fbk = Feedback.New(cmd.AggregateID.Id());
            try
            {
                Guard.Against.EngineNotInitialized(state);
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
                Log.Error($"{AppErrors.Error} - {e.InnerAndOuter()}");
            }
            return fbk;
        }

        public override IEnumerable<Event> Raise(Cmd cmd, Engine state)
        {
            return new[]
            {
                Evt.New(cmd.AggregateID, cmd.Payload)
            };
        }
    }


    [Topic(Topics.Cmd_v1)]
    public record Cmd(IID AggregateID, Contract.ChangeDetails.Payload Payload) 
        : CmdT<Contract.ChangeDetails.Payload>(AggregateID,
        Payload)
    {
        public static Cmd New(Schema.EngineID engineId, Contract.ChangeDetails.Payload payload) 
            => new(engineId, payload);
    }
    
    
    

    public static class Topics
    {
        public const string Cmd_v1 = "engine:change_details:v1";
        public const string Evt_v1 = "engine:details_changed:v1";
    }
    
    

    [Topic(Topics.Evt_v1)]
    public record Evt(IID AggregateID, 
        Contract.ChangeDetails.Payload Payload,
        EventMeta Meta) 
        : EvtT<Contract.ChangeDetails.Payload, EventMeta>(
            AggregateID, 
            TopicAtt.Get<Evt>(), 
            Payload, 
            Meta)
    {
        
        public static Evt New(
            IID engineID,
            Contract.ChangeDetails.Payload payload)
        {
            var meta = EventMeta.New(
                nameof(Aggregate), 
                engineID.Id());
            return new Evt(engineID, payload, meta);
        }
    }
}