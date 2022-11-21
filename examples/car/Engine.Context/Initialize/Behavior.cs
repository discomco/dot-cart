using System.Runtime.Serialization;
using Ardalis.GuardClauses;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Context.Behaviors;
using DotCart.Core;
using Engine.Context.Common;
using Engine.Contract;

namespace Engine.Context.Initialize;

public class ApplyEvt : ApplyEvtT<Common.Schema.Engine, IEvt>
{
    public override Common.Schema.Engine Apply(Common.Schema.Engine state, Event evt)
    {
        state.Id = evt.AggregateID.Id();
        state.Status = Schema.EngineStatus.Initialized;
        return state;
    }
}

public class TryCmd : TryCmdT<Cmd>
{
    public override IFeedback Verify(Cmd cmd)
    {
        var fbk = Feedback.New(cmd.AggregateID.Id());
        try
        {
            Guard.Against.EngineInitialized((Common.Schema.Engine)Aggregate.GetState());
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsError());
            Console.WriteLine(e);
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
            // Evt(, Payload.New(cmd.Payload.Engine))
        };
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

[Topic(Topics.EvtTopic)]
public interface IEvt : IEvtT<Contract.Initialize.Payload>
{
}

[Topic(Topics.CmdTopic)]
public record Cmd(
    IID AggregateID,
    Contract.Initialize.Payload Payload
) : CmdT<Contract.Initialize.Payload>(
    Topics.CmdTopic,
    AggregateID,
    Payload)
{
    public static Cmd New(
        IID aggregateID,
        Contract.Initialize.Payload payload)
    {
        return new Cmd(aggregateID, payload);
    }
}