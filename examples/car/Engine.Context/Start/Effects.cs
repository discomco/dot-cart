using DotCart.Context.Abstractions;
using DotCart.Context.Abstractions.Drivers;
using DotCart.Context.Effects;
using DotCart.Context.Schemas;
using DotCart.Contract;
using DotCart.Contract.Schemas;
using DotCart.Drivers.InMem;
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


    public class Responder : ResponderT<Spoke, ResponderDriver, Hope, Cmd>, IResponder
    {
        public Responder(IExchange exchange,
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(exchange,
            responderDriver,
            cmdHandler,
            hope2Cmd)
        {
        }
    }


    public interface IToMemDocProjection
    {
    }

    public class ToMemDocProjection : ProjectionT<Spoke, MemStore<Common.Schema.Engine>, Common.Schema.Engine, IEvt>,
        IToMemDocProjection
    {
        public ToMemDocProjection(IExchange exchange,
            IModelStore<Common.Schema.Engine> modelStore,
            Evt2State<Common.Schema.Engine, IEvt> evt2State) : base(exchange,
            modelStore,
            evt2State)
        {
        }
    }
}