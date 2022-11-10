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
using Serilog;

namespace DotCart.TestEnv.Engine;


public static partial class Inject
{

    public static IServiceCollection AddStartBehavior(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient<ITry, Start.TryCmd>()
            .AddTransient<IApply, Start.ApplyEvt>();
    }
    
    
    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddEngineAggregate()
            .AddAggregateBuilder()
            .AddTransient(_ => Start.evt2Cmd)
            .AddTransient<IAggregatePolicy, Start.StartOnInitializedPolicy>();
    }

    public static IServiceCollection AddStartEffects(this IServiceCollection services)
    {
        return services
            .AddStartedProjections()
            .AddStartEmitter()
            .AddStartResponder();
    }

    public static IServiceCollection AddStartEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Start._evt2Fact);
    }

    public static IServiceCollection AddStartedProjections(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddSingleton(_ => Start._evt2State)
            .AddEngineMemStore()
            .AddSingleton<IProjectionDriver<Schema.Engine>, EngineProjectionDriver>()
            .AddHostedService<Start.ToMemDocProjection>();
    }

    public static IServiceCollection AddStartResponder(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddStartOnInitializedPolicy()
            .AddCmdHandler()
            .AddStartHopeGenerator()
            .AddTransient(_ => Start._hope2Cmd)
            .AddSingleton<IResponderDriver<Start.Hope>, Start.ResponderDriver>()
            .AddHostedService<Start.Responder>();
    }


    public static IServiceCollection AddStartHopeGenerator(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Start._generateHope);
    }
}

public static class Start
{
    private const string HopeTopic = "engine.start.v1";
    private const string FactTopic = "engine.started.v1";
    private const string CmdTopic = "engine:start:v1";
    public const string EvtTopic = "engine:started:v1";


    #region Schema

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

        public static Exception New(string message)
        {
            return new Exception(message);
        }
    }

    #endregion


    #region Contract

    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }

    internal static GenerateHope<Hope> _generateHope => () =>
    {
        var pl = Payload.New;
        var aggId = TypedEngineID.New;
        return Hope.New(aggId.Value, pl.ToBytes());
    };

    [Topic(FactTopic)]
    public record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact
    {
        public static Fact New(string aggId, byte[] data)
        {
            return new Fact(aggId, data);
        }

        public static Fact New(string aggId, Payload payload)
        {
            return new Fact(aggId, payload.ToBytes());
        }
    }


    [Topic(HopeTopic)]
    public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
    {
        public static Hope New(string aggId, byte[] data)
        {
            return new Hope(aggId, data);
        }

        public static Hope New(string aggId, Payload payload)
        {
            return new Hope(aggId, payload.ToBytes());
        }
    }

    #endregion


    # region Behavior


    public class ApplyEvt : ApplyEvt<Schema.Engine, IEvt>
    {
        public override Schema.Engine Apply(Schema.Engine state, Event evt)
        {
            state.Status = (EngineStatus)((int)state.Status).SetFlag((int)EngineStatus.Started);
            return state;
        }
    }


    public class TryCmd : TryCmd<Cmd>, ITry<Cmd>
    {
       
        public override IFeedback Verify(Start.Cmd cmd)
        {
            var fbk = Feedback.Empty;
            try
            {
                Guard.Against.StateIsNotInitialized((Schema.Engine)Aggregate.GetState());
            }
            catch (Exception e)
            {
                fbk.SetError(e.AsError());
            }

            return fbk;
        }

        public override IEnumerable<Event> Raise(Cmd cmd)
        {
            return new[]
            {
                Event.New(
                    (EngineID)cmd.AggregateID, 
                    EvtTopic,
                    cmd.Payload, 
                    Aggregate.GetMeta(),
                    Aggregate.Version)
            };
        }

    }


    internal static Evt2Cmd<Initialize.IEvt, Cmd> evt2Cmd => evt => Cmd.New(evt.AggregateID, Payload.New);

    public class StartOnInitializedPolicy : AggregatePolicy<Initialize.IEvt, Cmd>
    {
        // protected override async Task Enforce(DotCart.Behavior.IEvt evt)
        // {
        //     var cmd = Cmd.New(evt.AggregateID, Payload.New);
        //     var fbk = await Aggregate.ExecuteAsync(cmd);
        //     Console.WriteLine(fbk.GetPayload<Schema.Engine>());
        // }

        public StartOnInitializedPolicy(
            ITopicMediator mediator,
            Evt2Cmd<Initialize.IEvt, Cmd> evt2Cmd)
            : base(mediator, evt2Cmd)
        {
        }
    }

    public interface ICmd : ICmd<Payload>
    {
    }

    [Topic(CmdTopic)]
    public record Cmd
        (IID AggregateID, Payload Payload) :
            Cmd<Payload>(CmdTopic, AggregateID, Payload), ICmd
    {
        public static Cmd New(IID aggID, Payload payload)
        {
            return new Cmd(aggID, payload);
        }
    }

    [Topic(EvtTopic)]
    public interface IEvt : IEvt<Payload>
    {
    }

    #endregion

    #region Effects

    internal static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    internal static readonly Evt2State<Schema.Engine, IEvt> _evt2State = (state, _) =>
    {
        ((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    };

    internal static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope =>
            Cmd.New(TypedEngineID.FromIdString(hope.AggId), hope.Data.FromBytes<Payload>());

    public class ResponderDriver : MemResponderDriver<Hope>, IResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }

    public class Responder : Responder<ResponderDriver, Hope, Cmd>
    {
        public Responder(
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }


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

    #endregion
}