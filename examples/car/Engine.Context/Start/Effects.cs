using DotCart.Client;
using DotCart.Client.Schemas;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Context.Schemas;
using DotCart.Drivers.InMem;
using Engine.Client.Schema;
using Engine.Client.Start;
using Engine.Context.Common.Drivers;

namespace Engine.Context.Start;

public static class Effects
{
    internal static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    internal static readonly Evt2State<Common.Schema.Engine, IEvt> _evt2State = (state, _) =>
    {
        ((int)state.Status).SetFlag((int)EngineStatus.Started);
        return state;
    };

    internal static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope =>
            Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());

    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }

    public class Responder : Responder<ResponderDriver, Hope, Cmd>
    {
        public Responder(
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }


    public class ToMemDocProjection : Projection<EngineProjectionDriver, Common.Schema.Engine, IEvt>
    {
        public ToMemDocProjection(ITopicMediator mediator,
            IProjectionDriver<Common.Schema.Engine> projectionDriver,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(mediator,
            projectionDriver,
            evt2State)
        {
        }
    }
}