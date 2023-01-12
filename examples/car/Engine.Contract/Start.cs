using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Start
{
    public static class Topics
    {
        public const string Hope_v1 = "engine.start.v1";
        public const string Fact_v1 = "engine.started.v1";
        public const string Cmd_v1 = "engine:start:v1";
        public const string Evt_v1 = "engine:started:v1";
    }

    [HopeTopic(Topics.Hope_v1)]
    [FactTopic(Topics.Fact_v1)]
    [CmdTopic(Topics.Cmd_v1)]
    [EvtTopic(Topics.Evt_v1)]
    public record Payload : IPayload
    {
        public static readonly Payload New = new();
    }
}