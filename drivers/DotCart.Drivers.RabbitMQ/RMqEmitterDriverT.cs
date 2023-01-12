using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.RabbitMq;
using RabbitMQ.Client;
using Serilog;

namespace DotCart.Drivers.RabbitMQ;

public class RMqEmitterDriverT<TPayload, TMeta> : DriverB, IRmqEmitterDriverT<TPayload, TMeta>
    where TPayload : IPayload
    where TMeta : IMetaB
{
    private readonly int _backoff = 100;
    private readonly IConnectionFactory _connFact;
    private readonly Fact2Msg<byte[], TPayload, TMeta> _fact2Msg;
    private readonly int _maxRetries = Polly.Config.MaxRetries;
    private IModel _channel;
    private IConnection _connection;


    public RMqEmitterDriverT(
        IConnectionFactory connFact,
        Fact2Msg<byte[], TPayload, TMeta> fact2Msg)
    {
        _connFact = connFact;
        _fact2Msg = fact2Msg;
    }


    public string Topic => FactTopicAtt.Get<TPayload>();

    public Task ConnectAsync()
    {
        return Task.Run(() =>
        {
            Log.Information($"{AppVerbs.Connecting} ");
            _connection = _connFact.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(FactTopicAtt.Get<TPayload>(), ExchangeType.Fanout);
        });
    }

    public Task EmitAsync(FactT<TPayload, TMeta> fact, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var body = _fact2Msg(fact);
            _channel.BasicPublish(Topic, "", null, body);
            return Task.CompletedTask;
        }, cancellationToken);
    }

    public override void Dispose()
    {
        if (_connection != null)
            _connection.Dispose();
        if (_channel != null)
            _channel.Dispose();
        base.Dispose();
    }
}