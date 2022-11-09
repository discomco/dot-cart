using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Drivers;
using DotCart.TestEnv.Engine.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine;



public static partial class Inject
{


    public static IServiceCollection AddThrottleUpBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, ThrottleUp.TryCmd>()
            .AddTransient<IApply, ThrottleUp.ApplyEvt>();
    }
    
    public static IServiceCollection AddThrottleUpEffects(this IServiceCollection services)
    {
        return services
            .AddThrottleUpProjections()
            .AddThrottleUpEmitter()
            .AddThrottleUpResponder();
    }

    public static IServiceCollection AddThrottleUpProjections(this IServiceCollection services)
    {
        return services
            .AddMediator()
            .AddSingleton<IProjectionDriver<Schema.Engine>, EngineProjectionDriver>()
            .AddEngineMemStore()
            .AddTransient(_ => ThrottleUp._evt2State)
            .AddHostedService<ThrottleUp.ToMemDocProjection>();
    }

    public static IServiceCollection AddThrottleUpEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => ThrottleUp._evt2Fact);
    }

    public static IServiceCollection AddThrottleUpResponder(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => ThrottleUp._hope2Cmd)
            .AddMemEventStore()
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddCmdHandler()
            .AddTransient(_ => ThrottleUp._generateHope)
            .AddSingleton<IResponderDriver<ThrottleUp.Hope>, ThrottleUp.ResponderDriver>()
            .AddHostedService<ThrottleUp.Responder>();
    }
}

public static class ThrottleUp
{
    public static Evt2Fact<Fact, IEvt> _evt2Fact =>
        evt => Fact.New(
            evt.AggregateID.Id(),
            evt.GetPayload<Payload>());
    public static Hope2Cmd<Cmd, Hope> _hope2Cmd => 
        hope => Cmd.New(TypedEngineID.FromIdString(hope.AggId), hope.GetPayload<Payload>());

    public const string FactTopic = "engine.throttled_up.v1";
    public const string HopeTopic = "engine.throttle_up.v1";
    
    public const string CmdTopic = "engine:trottle_up:v1";
    public const string EvtTopic = "engine:throttled_Up:v1";

    public record Payload(int Delta) : IPayload
    {
        public static Payload New(int delta) => new(delta);
        
    }


    public class TryCmd : TryCmd<Cmd>
    {
        public override IEnumerable<Event> Raise(Cmd cmd)
        {
            return new[]
            {
                Event.New((EngineID)cmd.AggregateID,
                    EvtTopic,
                    cmd.Payload,
                    Aggregate.GetMeta(),
                    Aggregate.Version)
            };
        }
        

        public override IFeedback Verify(ThrottleUp.Cmd cmd)
        {
            var fbk = Feedback.New(cmd.AggregateID);
            try
            {
                Guard.Against.EngineNotStarted((Schema.Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
            }
            return fbk;
        }
    }

    public class ApplyEvt : ApplyEvt<Schema.Engine, IEvt>
    {
        public override Schema.Engine Apply(Schema.Engine state, Event evt)
        {
            state.Power += evt.GetPayload<Payload>().Delta;
            return state;
        }
    }


    [Topic(CmdTopic)]
    public record Cmd(IID AggregateID, Payload Payload)
        : Cmd<Payload>(CmdTopic, AggregateID, Payload)
    {
        public static Cmd New(IID aggID, Payload payload)
        {
            return new Cmd(aggID, payload);
        }
    }

    [Topic(EvtTopic)]
    public interface IEvt: IEvt<Payload>
    {
        
    }
    

    public static GenerateHope<Hope> _generateHope => () =>
    {
        var engineID = TypedEngineID.New;
        var pl = Payload.New(Random.Shared.Next(20));
        return Hope.New(engineID.Value, pl);
    };

    public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
    {
        public static Hope New(string aggId, Payload payload) => new(aggId, payload.ToBytes());
    }

    public record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact
    {
        public static Fact New(string aggId, byte[] data) => new Fact(aggId, data);
        public static Fact New(string aggId, Payload payload) => new Fact(aggId, payload.ToBytes());
    }

    public class Exception : System.Exception
    {
        public Exception()
        {
        }

        protected Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Exception(string? message) : base(message)
        {
        }

        public Exception(string? message, System.Exception? innerException) : base(message, innerException)
        {
        }

        public static Exception New(string msg)
        {
            return new Exception(msg);
        }
    }
    
    public class Responder : Responder<MemResponderDriver<Hope>, Hope, Cmd>
    {
        public Responder(IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd,Hope> hope2Cmd) : base(responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }

    internal static Evt2State<Schema.Engine, IEvt> _evt2State = (state, evt) =>
    {
        state.Power += evt.GetPayload<Payload>().Delta;
        return state;
    };

    public class ToMemDocProjection : Projection<EngineProjectionDriver, Schema.Engine, IEvt>
    {
        public ToMemDocProjection(ITopicMediator mediator,
            IProjectionDriver<Schema.Engine> projectionDriver,
            Evt2State<Schema.Engine, IEvt> evt2State) : base(mediator,
            projectionDriver,
            evt2State)
        {
        }
    }


    public class ResponderDriver: MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        { }
    }
}