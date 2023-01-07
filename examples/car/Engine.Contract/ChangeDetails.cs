using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class ChangeDetails
{
    public static class Topics
    {
        public const string Hope_v1 = "engine.change_details.v1";
        public const string Cmd_v1 = "engine:change_details:v1";
        public const string Fact_v1 = "engine.details_changed.v1";
        public const string Evt_v1 = "engine:details_changed:v1";
    }

    [HopeTopic(Topics.Hope_v1)]
    [FactTopic(Topics.Fact_v1)]
    [CmdTopic(Topics.Cmd_v1)]
    [EvtTopic(Topics.Evt_v1)]
    public record Payload(Schema.Details Details) : IPayload
    {
        public Schema.Details Details { get; set; } = Details;

        public static Payload New(Schema.Details details)
        {
            return new Payload(details);
        }
    }

    public record DummyLoad : IPayload
    {
    }
}