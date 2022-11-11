using DotCart.Client;
using DotCart.Client.Schemas;
using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Core;
using DotCart.Drivers.InMem;
using Engine.Client.Initialize;
using Engine.Client.Schema;
using Engine.Context.Common.Drivers;

namespace Engine.Context.Initialize;

public static class Effects
{
    internal static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    internal static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());

    internal static readonly GenerateHope<Hope> _genHope =
        () =>
        {
            var details = Details.New("NewEngine");
            var aggID = EngineID.New();
            var pl = Payload.New(details);
            return Hope.New(aggID.Id(), pl.ToBytes());
        };

    public static Evt2State<Common.Schema.Engine, IEvt> _evt2State => (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Common.Schema.Engine>() == null) return state;
        state = evt.GetPayload<Common.Schema.Engine>();
        state.Id = evt.AggregateID.Id();
        state.Status = EngineStatus.Initialized;
        return state;
    };

    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }

    public class Responder : Responder<MemResponderDriver<Hope>, Hope, Cmd>
    {
        public Responder(
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(responderDriver, cmdHandler, hope2Cmd)
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