////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c)2023 DisComCo Sp.z.o.o. (http://discomco.pl)
////////////////////////////////////////////////////////////////////////////////////////////////
// Permission is hereby granted, free of charge,
// to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS",
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////


using DotCart.Abstractions.Actors;
using DotCart.Core;
using Serilog;
using static System.Threading.Tasks.Task;

namespace DotCart.Context.Actors;

public abstract class ActiveComponent : IActiveComponent
{
    private readonly object activateMutex = new();
    private readonly object startMutex = new();
    private CancellationTokenSource _cts;
    public string Name => GetName();

    public ComponentStatus Status { get; private set; }

    public Task Activate(CancellationToken stoppingToken = default)
    {
        if (Status == ComponentStatus.Active)
            return CompletedTask;
        lock (activateMutex)
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
                    Log.Information($"{AppFacts.Cancelled} [{GetType().Name}]");
                }
                catch (Exception e)
                {
                    Log.Fatal(e.InnerAndOuter());
                    Status = ComponentStatus.Inactive;
                }
            }, stoppingToken);
        }
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
        if (Status == ComponentStatus.Active)
            return CompletedTask;
        lock (startMutex)
        {
            return Run(async () =>
            {
                try
                {
                    Log.Information($"{AppFacts.Activated} [{Name}]");
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
    }

    private Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Run(() =>
        {
            if (!cancellationToken.IsCancellationRequested) return CompletedTask;
            Status = ComponentStatus.Inactive;
            Log.Information($"{AppFacts.Deactivated} {Name}");
            return StopActingAsync(cancellationToken);
        }, cancellationToken);
    }
}