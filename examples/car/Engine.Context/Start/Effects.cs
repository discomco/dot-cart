using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Context.Schemas;
using DotCart.Contract;
using DotCart.Contract.Schemas;
using DotCart.Drivers.InMem;
using Engine.Context.Common.Drivers;
using Engine.Contract.Schema;
using Engine.Contract.Start;

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

    public interface IResponder : IResponder<ResponderDriver, Hope, Cmd>
    {
    }


    public class Responder : Responder<Spoke, ResponderDriver, Hope, Cmd>, IResponder
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


    public interface IToMemDocProjection
    {
    }

    public class ToMemDocProjection : Projection<Spoke, MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToMemDocProjection
    {
        public ToMemDocProjection(ITopicMediator mediator,
            IModelStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(mediator,
            modelStore,
            evt2State)
        {
        }
    }
}