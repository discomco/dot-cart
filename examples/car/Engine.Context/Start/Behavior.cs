using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Engine.Context.Common;
using Engine.Contract;

namespace Engine.Context.Start;

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

public class ApplyEvt : ApplyEvtT<Common.Schema.Engine, IEvt>
{
    public override Common.Schema.Engine Apply(Common.Schema.Engine state, Event evt)
    {
        state.Status = (Schema.EngineStatus)((int)state.Status).SetFlag((int)Schema.EngineStatus.Started);
        return state;
    }
}

public class TryCmd : TryCmdT<Cmd>
{
    public override IFeedback Verify(Cmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            Guard.Against.StateIsNotInitialized((Common.Schema.Engine)Aggregate.GetState());
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
                (Schema.EngineID)cmd.AggregateID,
                Topics.EvtTopic,
                cmd.Payload,
                Aggregate.GetMeta(),
                Aggregate.Version)
        };
    }
}

public class StartOnInitializedPolicy : AggregatePolicy<Initialize.IEvt, Cmd>
{
    public StartOnInitializedPolicy(
        IExchange exchange,
        Evt2Cmd<Cmd, Initialize.IEvt> evt2Cmd)
        : base(exchange, evt2Cmd)
    {
    }
}

[Topic(Topics.CmdTopic)]
public record Cmd(IID AggregateID, Contract.Start.Payload Payload) : CmdT<Contract.Start.Payload>(Topics.CmdTopic,
    AggregateID, Payload)
{
    public static Cmd New(IID aggregateID, Contract.Start.Payload payload)
    {
        return new Cmd(aggregateID, payload);
    }
}

[Topic(Topics.EvtTopic)]
public interface IEvt : IEvtT<Contract.Start.Payload>
{
}