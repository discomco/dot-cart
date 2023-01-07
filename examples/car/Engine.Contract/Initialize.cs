using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Initialize
{
    public static class Topics
    {
        public const string Fact_v1 = "engine.initialized.v1";
        public const string Hope_v1 = "engine.initialize.v1";
        public const string Cmd_v1 = "engine:initialize:v1";
        public const string Evt_v1 = "engine:initialized:v1";
    }

    [HopeTopic(Topics.Hope_v1)]
    [FactTopic(Topics.Fact_v1)]
    [CmdTopic(Topics.Cmd_v1)]
    [EvtTopic(Topics.Evt_v1)]
    public record Payload(Schema.Details Details)
        : IPayload
    {
        public Schema.Details Details { get; } = Details;

        public static Payload New(Schema.Details details)
        {
            return new Payload(details);
        }
    }

    public record Dummyload
        : IPayload
    {
    }
}