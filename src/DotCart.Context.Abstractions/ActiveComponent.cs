using DotCart.Core;
using Serilog;

namespace DotCart.Context.Abstractions;

public abstract class ActiveComponent : IActiveComponent
{
    public bool IsRunning { get; private set; }

    public string Name => GetType().Name;

    public Task Activate(CancellationToken stoppingToken = default)
    {
        return Task.Run(async () =>
        {
            await PrepareAsync(stoppingToken);
            try
            {
                await StartAsync(stoppingToken).ConfigureAwait(false);
                while (!stoppingToken.IsCancellationRequested)
                {
                }

                await CleanupAsync(stoppingToken);
                return StopAsync(stoppingToken);
            }
            catch (Exception e)
            {
                Log.Fatal(e.InnerAndOuter());
                throw;
            }
        }, stoppingToken);
    }

    protected abstract Task CleanupAsync(CancellationToken cancellationToken);
    protected abstract Task PrepareAsync(CancellationToken cancellationToken = default);
    protected abstract Task StartActingAsync(CancellationToken cancellationToken = default);
    protected abstract Task StopActingAsync(CancellationToken cancellationToken = default);

    private Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            Log.Information($"Actor::{Name} ~> ACTIVATED");
            IsRunning = true;
            return StartActingAsync(cancellationToken);
        }, cancellationToken);
    }

    private Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            if (!cancellationToken.IsCancellationRequested) return Task.CompletedTask;
            IsRunning = false;
            Log.Information($"Actor::{Name}  ~> DEACTIVATED");
            return StopActingAsync(cancellationToken);
        }, cancellationToken);
    }
}