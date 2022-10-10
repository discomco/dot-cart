using DotCart.Contract;
using DotCart.Schema;
using DotCart.Schema.Tests;

namespace DotCart.Domain.Tests.Engine;

public partial class EngineAggregate :
    IExec<Schema.Tests.Engine, Start.Cmd>,
    IApply<Schema.Tests.Engine, Start.Evt>
{
    public IFeedback Verify(Schema.Tests.Engine state, Start.Cmd cmd)
    {
        var fbk = Feedback.Empty;
        try
        {
            if (!state.Status.HasFlag(EngineStatus.Initialized))
                throw new NotInitializedException("engine is not initialized");
        }
        catch (Exception e)
        {
            fbk.SetError(e.AsApiError());
        }
        return fbk;
    }

    public IEnumerable<IEvt> Exec(Schema.Tests.Engine state, Start.Cmd cmd)
    {
        return new[]
        {
            new Start.Evt(cmd.AggregateID, cmd.Payload)
        };
    }

    public Schema.Tests.Engine Apply(Schema.Tests.Engine state, Start.Evt evt)
    {
        state.Status = (EngineStatus)((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    }
}

public static class Start
{
    public const string CmdTopic = "engine:start:v1";
    public const string EvtTopic = "engine:started:v1";
    public record Payload : IPayload;
    public record Cmd
        (EngineID AggregateID, Payload Payload) :
            Cmd<EngineID, Payload>(CmdTopic, AggregateID, Payload);

    public record Evt
        (EngineID AggregateID, Payload Payload) :
            Evt<EngineID, Payload>(EvtTopic, AggregateID, Payload);
}