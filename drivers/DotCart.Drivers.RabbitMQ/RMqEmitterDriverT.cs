using System.Text.Json;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Polly.Retry;
using RabbitMQ.Client;
using Serilog;

namespace DotCart.Drivers.RabbitMQ;

public abstract class RMqEmitterDriverT<TFact> : DriverB, IEmitterDriverT<TFact, byte[]>
    where TFact : IFact
{
    private readonly int _backoff = 100;
    private readonly IConnectionFactory _connFact;
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private readonly IModel _channel;
    private readonly IConnection _connection;


    public string Topic => TopicAtt.Get<TFact>();

    public override void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
        base.Dispose();
    }


    protected RMqEmitterDriverT(
        IConnectionFactory connFact,
        AsyncRetryPolicy retryPolicy = null) 
    {
        _connFact = connFact;
        _connection = _connFact.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(TopicAtt.Get<TFact>(), ExchangeType.Fanout);
    }

    // TODO: Add retry policy
    public async Task EmitAsync(TFact fact, CancellationToken cancellationToken = default)
    {
        Log.Debug($"[{Topic}]-EMIT Fact[{fact.Topic}]");
        var body = await ToTarget(fact, cancellationToken).ConfigureAwait(false);
        _channel.BasicPublish(Topic, "", null, body);
    }

    public async Task<byte[]> ToTarget(TFact fact, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeToUtf8Bytes(fact);
    }
}