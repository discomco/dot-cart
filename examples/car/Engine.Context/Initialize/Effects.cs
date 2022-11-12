using DotCart.Context.Behaviors;
using DotCart.Context.Effects;
using DotCart.Context.Effects.Drivers;
using DotCart.Contract.Schemas;
using DotCart.Core;
using DotCart.Drivers.InMem;
using Engine.Context.Common.Drivers;
using Engine.Contract.Initialize;
using Engine.Contract.Schema;

namespace Engine.Context.Initialize;

public static class Mappers
{
    public static readonly Evt2Fact<Fact, IEvt> _evt2Fact =
        evt => Fact.New(evt.AggregateID.Id(), evt.GetPayload<Payload>());

    public static readonly Hope2Cmd<Cmd, Hope> _hope2Cmd =
        hope => Cmd.New(hope.AggId.IDFromIdString(), hope.GetPayload<Payload>());
    
    public static Evt2State<Common.Schema.Engine, IEvt> _evt2State => (state, evt) =>
    {
        if (evt == null) return state;
        if (evt.GetPayload<Common.Schema.Engine>() == null) return state;
        state = evt.GetPayload<Common.Schema.Engine>();
        state.Id = evt.AggregateID.Id();
        state.Status = EngineStatus.Initialized;
        return state;
    };

   
}

public static class Effects
{
    public interface IResponder: IReactor<Spoke>
    {
        
    }
    [Name("Engine.Initialize.Responder")]
    public class Responder : Responder<Spoke, MemResponderDriver<Hope>, Hope, Cmd>, IResponder
    {
        public Responder(
            IResponderDriver<Hope> responderDriver,
            ICmdHandler cmdHandler,
            Hope2Cmd<Cmd, Hope> hope2Cmd) : base(responderDriver, cmdHandler, hope2Cmd)
        {
        }

    }

    public interface IMemDocProjection
    {
        
    }
    
    [Name("Engine.Initialize.ToMemDocProjection")]
    public class ToMemDocProjection : Projection<Spoke,EngineProjectionDriver, Common.Schema.Engine, IEvt>, IMemDocProjection
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