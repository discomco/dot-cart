using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Context.Behaviors;
using DotCart.Context.Schemas;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using DotCart.Core;
using Engine.Context.Common;
using Engine.Contract.Schema;
using Engine.Contract.Start;

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

public class ApplyEvt : ApplyEvt<Common.Schema.Engine, IEvt>
{
    public override Common.Schema.Engine Apply(Common.Schema.Engine state, Event evt)
    {
        state.Status = (EngineStatus)((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    }
}

public class TryCmd : TryCmd<Cmd>
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
                (EngineID)cmd.AggregateID,
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
        ITopicMediator mediator,
        Evt2Cmd<Initialize.IEvt, Cmd> evt2Cmd)
        : base(mediator, evt2Cmd)
    {
    }
}

[Topic(Topics.CmdTopic)]
public record Cmd(IID AggregateID, Payload Payload) : Cmd<Payload>(Topics.CmdTopic, AggregateID, Payload)
{
    public static Cmd New(IID aggregateID, Payload payload)
    {
        return new Cmd(aggregateID, payload);
    }
}

[Topic(Topics.EvtTopic)]
public interface IEvt : IEvt<Payload>
{
}