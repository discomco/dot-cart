using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Actors;

public interface IActorT<in TSpoke> : IActor
{
    void SetSpoke(ISpokeB spoke);
}

public interface IActor : IActiveComponent
{
    Task HandleCast(IMsg msg, CancellationToken cancellationToken = default);
    Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default);
}

[Flags]
public enum ComponentStatus
{
    Inactive = 0,
    Active = 1
}

// public static class ComponentStatusHelpers
// {
//     public static ComponentStatus SetFlag(this ComponentStatus status, ComponentStatus flag)
//     {
//         var currentStatus = (int)status;
//         return (ComponentStatus) currentStatus.SetFlag((int)flag);
//     }
//
//     public static ComponentStatus UnsetFlag(this ComponentStatus status, ComponentStatus flag)
//     {
//         var currentStatus = (int)status;
//         return (ComponentStatus)currentStatus.UnsetFlag((int)flag);
//     }
//     
//     
//     
// }

public interface IActiveComponent : IDisposable
{
    string Name { get; }
    ComponentStatus Status { get; }
    Task Activate(CancellationToken cancellationToken = default);
    Task Deactivate(CancellationToken cancellationToken = default);
}