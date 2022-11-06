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

public partial class Aggregate :
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

public static partial class Inject
{
    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddTransient(_ => Start.evt2Cmd)
            .AddTransient<IDomainPolicy, Start.StartOnInitializedPolicy>();
    }

    public static IServiceCollection AddStartEffects(this IServiceCollection services)
    {
        return services
            .AddStartedToMemDocProjection()
            .AddStartEmitter()
            .AddStartResponder();
    }

    public static IServiceCollection AddStartEmitter(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => Start._evt2Fact);
    }

    public static IServiceCollection AddStartedToMemDocProjection(this IServiceCollection services)
    {
        return services
            .AddSingleton(_ => Start._evt2State)
            .AddEngineMemStore()
            .AddSingleton<IProjectionDriver<Schema.Engine>, EngineProjectionDriver>()
            .AddHostedService<Start.ToMemDocProjection>();
    }

    public static IServiceCollection AddStartResponder(this IServiceCollection services)
    {
        return services
            .AddMemEventStore()
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
    private const string EvtTopic = "engine:started:v1";


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
        var aggId = EngineID.New;
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

    internal static Evt2Cmd<Initialize.Evt, Cmd> evt2Cmd => evt => Cmd.New(evt.AggregateID, Payload.New);

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

    #endregion

    #region Effects

    internal static readonly Evt2Fact<Fact, Evt> _evt2Fact =
        evt => Fact.New(evt.AggregateId, evt.Payload);

    internal static readonly Evt2State<Schema.Engine, Evt> _evt2State = (state, _) =>
    {
        ((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    };

    internal static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope =>
            Cmd.New(EngineID.FromIdString(hope.AggId), hope.Data.FromBytes<Payload>());

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
        public Responder(IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(responderDriver,
            cmdHandler,
            hope2Cmd)
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