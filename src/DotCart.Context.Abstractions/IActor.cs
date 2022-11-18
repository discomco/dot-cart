using DotCart.Contract.Dtos;

namespace DotCart.Context.Abstractions;

public interface IActor<in TSpoke> : IActor
{
    void SetSpoke(ISpokeB spoke);
}

public interface IActor<in TSpoke, TActor> : IActor<TSpoke>
    where TActor : IActor
    where TSpoke : ISpokeT<TSpoke>
{
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

public interface IActiveComponent
{
    string Name { get; }
    ComponentStatus Status { get; }
    Task Activate(CancellationToken cancellationToken = default);
    Task Deactivate(CancellationToken cancellationToken = default);
}