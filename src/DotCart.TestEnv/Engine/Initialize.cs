using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Effects;
using DotCart.TestEnv.Engine.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace DotCart.TestEnv.Engine;

public partial class Aggregate
    : ITry<Initialize.Cmd>,
        IApply<Schema.Engine, Initialize.Evt>
{
    public IState Apply(Schema.Engine state, Initialize.Evt evt)
    {
        state.Id = evt.AggregateID.Value;
        state.Status = EngineStatus.Initialized;
        return state;
    }

    public IFeedback Verify(Initialize.Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID);
        try
        {
            Guard.Against.EngineInitialized(_state);
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Console.WriteLine(e);
        }

        return fbk;
    }

    public IEnumerable<IEvt> Raise(Initialize.Cmd cmd)
    {
        return new[]
        {
            new Initialize.Evt(cmd.AggregateID, Initialize.Payload.New(cmd.Payload.Engine))
        };
    }
}

public static partial class Inject
{
    public static IServiceCollection AddInitializeEffects(this IServiceCollection services)
    {
        return services
            .AddInitializeResponder()
            .AddInitializedEmitter()
            .AddInitializedToMemDocProjection();
    }

    public static IServiceCollection AddInitializedEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Initialize._evt2Fact);

    }


    public static IServiceCollection AddInitializedToMemDocProjection(this IServiceCollection services)
    {
        return services
            .AddTopicMediator()
            .AddEngineMemStore()
            .AddSingleton(_ => Initialize._evt2State)
            .AddSingleton<IProjectionDriver<Schema.Engine>, EngineProjectionDriver>()
            .AddHostedService<Initialize.ToMemDocProjection>();
    }


    public static IServiceCollection AddInitializeResponder(this IServiceCollection services)
    {
        return services
            .AddMemEventStore()
            .AddAggregateBuilder()
            .AddEngineAggregate()
            .AddCmdHandler()
            .AddTransient(_ => Initialize._genHope)
            .AddTransient(_ => Initialize._hope2Cmd)
            .AddSingleton<IResponderDriver<Initialize.Hope>, Initialize.ResponderDriver>()
            .AddHostedService<Initialize.Responder>();
    }
}

public static class Initialize
{
    public const string FactTopic = "test.engine.initialized.v1";
    public const string HopeTopic = "test.engine.initialize.v1";
    public const string CmdTopic = "test:engine:initialize:v1";
    public const string EvtTopic = "test:engine:initialized:v1";

    #region Contract Region =====================================
    public record Payload : IPayload
    {
        public Payload()
        {
        }

        private Payload(Schema.Engine engine)
        {
            Engine = engine;
        }

        public Schema.Engine Engine { get; }

        public static Payload New(Schema.Engine engine)
        {
            return new Payload(engine);
        }
    }

    [Topic(HopeTopic)]
    public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
    {
        public static Hope New(string aggId, byte[] data)
        {
            return new Hope(aggId, data);
        }
    }

    [Topic(FactTopic)]
    public record Fact(string AggId, byte[] Data) : Dto(AggId, Data), IFact
    {
        public static Fact New(string aggId, byte[] data) => new(aggId, data);
        public static Fact New(string aggId, Payload payload) => new(aggId, payload.ToBytes());
    }
    #endregion

    #region Behavior Region =================================
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
    public interface IEvt : IEvt<Payload>
    {
    }

    [Topic(EvtTopic)]
    public record Evt(IID AggregateID, Payload Payload)
        : Evt<Payload>(EvtTopic, AggregateID, Payload), IEvt
    {
        public static Evt New(IID engineId, Payload initPayload)
        {
            return new Evt(engineId, initPayload);
        }
    }

    public interface ICmd : ICmd<Payload>
    {
    }

    [Topic(CmdTopic)]
    public record Cmd(IID AggregateID, Payload Payload)
        : Cmd<Payload>(CmdTopic, AggregateID, Payload), ICmd
    {
        public static Cmd New(IID engineId, Payload payload)
        {
            return new Cmd(engineId, payload);
        }
    }
    #endregion

    #region Effects Region ===========================================

    internal static readonly Evt2Fact<Fact, Evt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Value, evt.Payload);


    internal static readonly Hope2Cmd<Cmd,Hope> _hope2Cmd =
        hope => Cmd.New(
            ID<EngineID>.FromIdString(hope.AggId),
            hope.Data.FromBytes<Payload>()
        );

    internal static readonly GenerateHope<Hope> _genHope =
        () =>
        {
            var eng = Schema.Engine.Ctor();
            var aggID = EngineID.New;
            var pl = Payload.New(eng);
            return Hope.New(aggID.Value, pl.ToBytes());
        };

    public static Evt2State<Schema.Engine, Evt> _evt2State => (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Schema.Engine>() == null) return state;
        state = evt.GetPayload<Schema.Engine>();
        state.Id = evt.AggregateId;
        state.Status = EngineStatus.Initialized;
        return state;
    };

    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
            
        }
    }

    public class Responder : Responder<MemResponderDriver<Hope>, Hope, Cmd>
    {
        public Responder(
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd,Hope> hope2Cmd) : base(responderDriver, cmdHandler, hope2Cmd)
        {
        }
    }


    public class ToMemDocProjection : Projection<EngineProjectionDriver, Schema.Engine, Evt>
    {
        public ToMemDocProjection(ITopicMediator mediator,
            IProjectionDriver<Schema.Engine> projectionDriver,
            Evt2State<Schema.Engine, Evt> evt2State) : base(mediator,
            projectionDriver,
            evt2State)
        {
        }
    }

    #endregion
}