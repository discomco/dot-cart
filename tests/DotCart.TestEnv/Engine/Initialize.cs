using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using DotCart.Effects;
using DotCart.Effects.Drivers;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Behavior;
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
    public const string CmdTopic = "test:engine:initialize:v1";
    public const string EvtTopic = "test:engine:initialized:v1";

    public record Payload : IPayload
    {
        public Payload()
        {
        }

        private Payload(Engine.Schema.Engine engine)
        {
            Engine = engine;
        }

        public Engine.Schema.Engine Engine { get; }

        public static Payload New(Engine.Schema.Engine engine)
        {
            return new Payload(engine);
        }
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

    public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
    {
        public static Hope New(string aggId, byte[] data)
        {
            return new Hope(aggId, data);
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

    public record Cmd(IID AggregateID, Payload Payload)
        : Cmd<Payload>(CmdTopic, AggregateID, Payload), ICmd
    {
        public static Cmd New(IID engineId, Payload payload)
        {
            return new Cmd(engineId, payload);
        }
    }


    #region Effects

    internal static readonly Hope2Cmd<Hope, Cmd> _hope2Cmd =
        hope => Cmd.New(
            ID<EngineID>.FromIdString(hope.AggId),
            hope.Data.FromBytes<Payload>()
        );

    internal static readonly GenerateHope<Hope> _genHope =
        () =>
        {
            var eng = TestEnv.Engine.Schema.Engine.Ctor();
            var aggID = EngineID.New;
            var pl = Payload.New(eng);
            return Hope.New(aggID.Value, pl.ToBytes());
        };

    public class ResponderDriver : MemResponderDriver<Hope>, IResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }
    }

    public class Responder : Responder<MemResponderDriver<Hope>, Hope, Cmd>
    {
        public Responder(
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Hope, Cmd> hope2Cmd) : base(responderDriver, cmdHandler, hope2Cmd)
        {
        }
    }

    #endregion
}