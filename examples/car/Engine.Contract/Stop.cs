using DotCart.Abstractions.Schema;
using DotCart.Core;

namespace Engine.Contract;

public static class Stop
{
    public static class Topics
    {
        public const string Fact_v1 = "engine.stopped.v1";
        public const string Hope_v1 = "engine.stop.v1";
        public const string Evt_v1 = "engine:stopped:v1";
        public const string Cmd_v1 = "engine:stop:v1";
    }

    [HopeTopic(Topics.Hope_v1)]
    [FactTopic(Topics.Fact_v1)]
    [CmdTopic(Topics.Cmd_v1)]
    [EvtTopic(Topics.Evt_v1)]
    public record Payload : IPayload
    {
        public static Payload New()
        {
            return new Payload();
        }
    }

}