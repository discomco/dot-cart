using DotCart.Core;
using Serilog;

namespace DotCart.Context.Abstractions;

public abstract class ActiveComponent : IActiveComponent
{
    public bool IsRunning { get; private set; }

    public string Name => GetType().Name;

    public Task<bool> Activate(CancellationToken stoppingToken = default)
    {
        return Task<bool>.Run<bool>(async () =>
        {
            await PrepareAsync(stoppingToken).ConfigureAwait(false);
            try
            {
                IsRunning = await StartAsync(stoppingToken).ConfigureAwait(false);
                while (!stoppingToken.IsCancellationRequested)
                {
                }

                await CleanupAsync(stoppingToken).ConfigureAwait(false);
                await StopAsync(stoppingToken).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Log.Fatal(e.InnerAndOuter());
                IsRunning = false;
            }
            return IsRunning;
        }, stoppingToken);
    }

    protected abstract Task CleanupAsync(CancellationToken cancellationToken);
    protected abstract Task PrepareAsync(CancellationToken cancellationToken = default);
    protected abstract Task StartActingAsync(CancellationToken cancellationToken = default);
    protected abstract Task StopActingAsync(CancellationToken cancellationToken = default);

    private Task<bool> StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                Log.Information($"Actor::{Name} ~> ACTIVATED");
                await StartActingAsync(cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                Log.Fatal(e.InnerAndOuter());
                return false;
            }
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