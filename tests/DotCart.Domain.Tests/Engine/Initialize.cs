using DotCart.Contract;
using DotCart.Schema.Tests;

namespace DotCart.Domain.Tests.Engine;

public partial class EngineAggregate
    : IExec<Schema.Tests.Engine, Initialize.Cmd>,
        IApply<Schema.Tests.Engine, Initialize.Evt>
{
    public IFeedback Verify(Schema.Tests.Engine state, Initialize.Cmd cmd)
    {
        return Feedback.Empty;
    }

    public IEnumerable<IEvt> Exec(Schema.Tests.Engine state, Initialize.Cmd cmd)
    {
        return new []
        {
            new Initialize.Evt(cmd.AggregateID, state)
        };
    }


    public Schema.Tests.Engine Apply(Schema.Tests.Engine state, Initialize.Evt evt)
    {
        state.Id = evt.AggregateID.Value;
        state.Status = EngineStatus.Initialized;
        return state;
    }
}

public static class Initialize
{
    private const string CmdTopic = "test:engine:initialize:v1";
    private const string EvtTopic = "test:engine:initialized:v1";

    public record Cmd
        (EngineID AggregateID, Schema.Tests.Engine Payload)
        : Cmd<EngineID, Schema.Tests.Engine>(CmdTopic, AggregateID, Payload);

    public record Evt
        (EngineID AggregateID, Schema.Tests.Engine Payload)
        : Evt<EngineID, Schema.Tests.Engine>(EvtTopic, AggregateID, Payload);
}