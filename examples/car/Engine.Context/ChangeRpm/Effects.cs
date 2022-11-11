using DotCart.Client;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Drivers.InMem;
using Engine.Client.ChangeRpm;
using Engine.Context.Common.Drivers;

namespace Engine.Context.ChangeRpm;

public static class Effects
{
    public class Responder : Responder<MemResponderDriver<Hope>, Hope, Cmd>
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


    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}