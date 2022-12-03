using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Abstractions.Actors;

public abstract class ActiveComponent : IActiveComponent
{
    private CancellationTokenSource _cts;
    public string Name => GetName();

    public ComponentStatus Status { get; private set; }

    public Task Activate(CancellationToken stoppingToken = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        return Run(async () =>
        {
            await PrepareAsync(_cts.Token).ConfigureAwait(false);
            try
            {
                await StartAsync(_cts.Token).ConfigureAwait(false);
                if (Status == ComponentStatus.Active)
                    LoopAsync(_cts.Token);
            }
            catch (OperationCanceledException e)
            {
                Status = ComponentStatus.Inactive;
                Log.Information($"ActiveComponent [{GetType()}] is Canceled");
            }
            catch (Exception e)
            {
                Log.Fatal(e.InnerAndOuter());
                Status = ComponentStatus.Inactive;
            }
        }, stoppingToken);
    }

    public Task Deactivate(CancellationToken cancellationToken = default)
    {
        if (!cancellationToken.IsCancellationRequested)
            _cts.Cancel();
        return CompletedTask;
    }

    public virtual void Dispose()
    {
        if (_cts != null)
            _cts.Dispose();
    }

    protected virtual string GetName()
    {
        return NameAtt.Get(this);
    }

    private Task LoopAsync(CancellationToken stoppingToken)
    {
        return Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
                Thread.Sleep(1000);
            await CleanupAsync(stoppingToken).ConfigureAwait(false);
            await StopAsync(stoppingToken).ConfigureAwait(false);
        }, stoppingToken);
    }

    protected abstract Task CleanupAsync(CancellationToken cancellationToken);
    protected abstract Task PrepareAsync(CancellationToken cancellationToken = default);
    protected abstract Task StartActingAsync(CancellationToken cancellationToken = default);
    protected abstract Task StopActingAsync(CancellationToken cancellationToken = default);

    private Task StartAsync(CancellationToken cancellationToken)
    {
        return Run(async () =>
        {
            try
            {
                var activated = "ACTIVATED".AsFact();
                Log.Information($"{activated} [{Name}]");
                Status = ComponentStatus.Active;
                await StartActingAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Fatal(e.InnerAndOuter());
                Status = ComponentStatus.Inactive;
            }
        }, cancellationToken);
    }

    private Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            if (!cancellationToken.IsCancellationRequested) return CompletedTask;
            Status = ComponentStatus.Inactive;
            Log.Information($"Actor::{Name}  ~> DEACTIVATED");
            return StopActingAsync(cancellationToken);
        }, cancellationToken);
    }
}