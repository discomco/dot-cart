using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Drivers.InMem;
using DotCart.Schema;
using DotCart.TestEnv.Engine.Behavior;
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

public static class Start
{
    private const string CmdTopic = "engine:start:v1";
    private const string EvtTopic = "engine:started:v1";
    public class Exception : System.Exception
    {
        public static Exception New(string message)
        {
            return new Exception(message);
        }

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
    }
    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }
    private static GenerateHope<Hope> _generateHope => () => {
        var pl = Payload.New;
        var aggId = EngineID.New;
        return Hope.New(aggId.Value, pl.ToBytes());
    };
    public record Hope(string AggId, byte[] Data) : Dto(AggId, Data), IHope
    {
        public static Hope New(string aggId, byte[] data) => new(aggId, data);
    }
    private static Evt2Cmd<Initialize.Evt, Cmd> evt2Cmd => evt => Cmd.New(evt.AggregateID, Payload.New);
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
    public static IServiceCollection AddStartHopeGenerator(this IServiceCollection services)
    {
        return services
            .AddTransient(_ => _generateHope);
    }
    public static IServiceCollection AddStartOnInitializedPolicy(this IServiceCollection services)
    {
        return services
            .AddAggregateBuilder()
            .AddTransient(_ => evt2Cmd)
            .AddTransient<IDomainPolicy, StartOnInitializedPolicy>();
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