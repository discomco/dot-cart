using DotCart.Abstractions;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Microsoft.Extensions.DependencyInjection;
using static System.Threading.Tasks.Task;

namespace DotCart.Actors;

public static partial class Inject
{
    public static IServiceCollection AddMemProjector(this IServiceCollection services)
    {
        return services
            .AddSingletonExchange()
            .AddSingleton<IProjector, MemProjector>()
            .AddSingleton<IMemProjector, MemProjector>();
    }
}

/// <summary>
///     MemProjector is an in-memory Unit of Effect,
///     that is meant to be injected into an AggregateStore
///     It offers a straightforward interface to project events onto the Exchange
/// </summary>
[Name(MemProjectorName)]
internal sealed class MemProjector : ActorB, IMemProjector
{
    private const string MemProjectorName = "dotcart:mem_projector";

    public MemProjector(IExchange exchange) : base(exchange)
    {
    }

    public override Task HandleCast(IMsg msg, CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            _exchange.Publish(TopicAtt.Get(msg), msg, cancellationToken);
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