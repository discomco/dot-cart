using System.Text.Json;
using DotCart.Behavior;
using DotCart.Contract;
using DotCart.Effects;
using NATS.Client;
using Serilog;

namespace DotCart.Drivers.NATS;

public abstract class ResponderDriver<THope, TCmd> : IResponderDriver
    where THope : IHope
    where TCmd : ICmd
{
    private readonly IEncodedConnection _conn;
    private string _logMessage;
    private IAsyncSubscription _subscription;

    protected ResponderDriver(
        IEncodedConnection conn,
        ICmdHandler cmdHandler)
    {
        _conn = conn;
        _conn.OnSerialize = o =>
        {
            var res = o.ToBytes();
            return res;
        };
        _conn.OnDeserialize = data =>
        {
            try
            {
                return data.ToObject<THope>();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        };
    }

    public string Topic => GetTopic();

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            WaitForConnection();
            _logMessage = $"[{Topic}]-RSP on [{JsonSerializer.Serialize(_conn.DiscoveredServers)}]";
            _logger?.Debug(_logMessage);
            _logMessage = "";
            _subscription = _conn.SubscribeAsync(Topic, async (sender, args) =>
            {
                if (args.ReceivedObject is not THope hope) return;
                _logger?.Debug($"[{Topic}]-HOPE {hope.AggregateId}");
                var cmd = ToCommand(hope);
                var fbk = await _actor.HandleAsync(cmd);
                _conn.Publish(args.Message.Reply, fbk);
                _conn.Flush();
                _logger?.Debug($"[{Topic}]-FEEDBACK {fbk.ErrorState.IsSuccessful} ");
            });
        }
        catch (Exception e)
        {
            _logMessage = $"[{Topic}]-ERR {JsonSerializer.Serialize(e.AsApiError())}";
            _logger.Fatal(_logMessage);
            _subscription.DrainAsync();
        }

        return Task.CompletedTask;
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await StopRespondingAsync(cancellationToken);
    }


    private string GetTopic()
    {
        var attrs = (TopicAttribute[])typeof(THope).GetCustomAttributes(typeof(TopicAttribute), true);
        return attrs.Length > 0 ? attrs[0].Id : throw new Exception($"No Topic Defined on {typeof(THope)}!");
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
        }

        return Task.CompletedTask;
    }


    protected abstract TCmd ToCommand(THope hope);

    private async Task StopRespondingAsync(CancellationToken cancellationToken)
    {
        if (_conn.State != ConnState.CONNECTED) return;
        _logMessage = $"[CONN]-DRN [{_conn.ConnectedUrl}]";
        _logger?.Debug(_logMessage);
        await _conn.DrainAsync().ConfigureAwait(false);
        _logMessage = $"[CONN]-CLS [{_conn.ConnectedUrl}]";
        _logger?.Debug(_logMessage);
        _conn.Close();
    }

    private void WaitForConnection()
    {
        Task.Run(() =>
        {
            while (_conn.State != ConnState.CONNECTED)
                _logger.Information($"Waiting for Connection. State: {_conn.State}");
        });
    }
}