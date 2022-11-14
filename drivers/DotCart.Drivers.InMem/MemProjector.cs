using DotCart.Context.Abstractions;
using DotCart.Context.Effects;
using DotCart.Contract.Dtos;
using DotCart.Core;
using DotCart.Drivers.Mediator;
using Microsoft.Extensions.DependencyInjection;
using static System.Threading.Tasks.Task;

namespace DotCart.Drivers.InMem;

public static partial class Inject
{
    public static IServiceCollection AddMemProjector(this IServiceCollection services)
    {
        return services
            .AddExchange()
            .AddSingleton<IProjector, MemProjector>()
            .AddSingleton<IMemProjector, MemProjector>();
    }
}

/// <summary>
///     MemProjector is an in-memory Unit of Effect,
///     that is meant to be injected into an AggregateStore
///     It offers a straightforward interface to project events onto the TopicMediator
/// </summary>
[Topic("memory")]
internal sealed class MemProjector : Actor, IMemProjector
{
    public MemProjector(IExchange exchange) : base(exchange)
    {
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            _exchange.Publish(msg.Topic, msg, cancellationToken);
            return CompletedTask;
        }, cancellationToken);
    }

    public override Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default)
    {
        return (Task<IMsg>)CompletedTask;
    }

    protected override Task CleanupAsync(CancellationToken cancellationToken)
    {
        return CompletedTask;
    }

    protected override Task StartActingAsync(CancellationToken cancellationToken = default)
    {
        return CompletedTask;
    }

    protected override Task StopActingAsync(CancellationToken cancellationToken = default)
    {
        return CompletedTask;
    }
}

public interface IMemProjector : IProjector
{
}