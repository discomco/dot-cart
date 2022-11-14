using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Context.Abstractions;
using DotCart.Context.Behaviors;
using DotCart.Contract.Dtos;
using DotCart.Contract.Schemas;
using DotCart.Core;
using Engine.Context.Common;
using Engine.Contract.ChangeRpm;
using Engine.Contract.Schema;

namespace Engine.Context.ChangeRpm;

[Topic(Topics.CmdTopic)]
public record Cmd(IID AggregateID, Payload Payload) : Cmd<Payload>(Topics.CmdTopic, AggregateID, Payload), ICmd
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

public class TryCmd : TryCmd<Cmd>
{
    public override IEnumerable<Event> Raise(Cmd cmd)
    {
        return new[]
        {
            Event.New((EngineID)cmd.AggregateID,
                Topics.EvtTopic,
                cmd.Payload,
                Aggregate.GetMeta(),
                Aggregate.Version)
        };
    }


    public override IFeedback Verify(Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID.Id());
        try
        {
            Guard.Against.EngineNotStarted((Common.Schema.Engine)Aggregate.GetState());
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
        }

        return fbk;
    }
}

public class ApplyEvt : ApplyEvt<Common.Schema.Engine, IEvt>
{
    public override Common.Schema.Engine Apply(Common.Schema.Engine state, Event evt)
    {
        state.Power += evt.GetPayload<Payload>().Delta;
        return state;
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