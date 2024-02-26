using Ardalis.GuardClauses;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.RabbitMq;
using DotCart.Schema;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace DotCart.Drivers.RabbitMQ;

public class RMqListenerDriverT<TFactPayload, TFactMeta>
    : DriverB, IRmqListenerDriverT<TFactPayload>
    where TFactPayload : IPayload
    where TFactMeta : IMetaB
{
    private readonly IConnectionFactory _connFact;
    private readonly Msg2Fact<TFactPayload, TFactMeta, byte[]> _msg2Fact;

    private IModel _channel;
    private IConnection _connection;
    private AsyncEventingBasicConsumer _consumer;


    public RMqListenerDriverT(
        IConnectionFactory connFact,
        Msg2Fact<TFactPayload, TFactMeta, byte[]> msg2Fact)
    {
        _connFact = connFact;
        _msg2Fact = msg2Fact;
    }

    public override void Dispose()
    {
        if (_connection != null)
            _connection.Dispose();
        if (_channel != null)
            _channel.Dispose();
        base.Dispose();
    }

    public Task StartListeningAsync(CancellationToken cancellationToken = default)
    {
        _connection = _connFact.CreateConnection();
        _channel = _connection.CreateModel();
        Log.Debug($"{AppVerbs.Subscribing} [{FactTopicAtt.Get<TFactPayload>()}]");
        _channel.ExchangeDeclare(Topic, ExchangeType.Fanout);
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queueName, Topic, "");
        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.Received += FactReceived;
        _channel.BasicConsume(queueName, true, _consumer);
        return Task.CompletedTask;
    }

    public Task StopListeningAsync(CancellationToken cancellationToken = default)
    {
        _connection.Close();
        _channel.Close();
        _consumer.Received -= FactReceived;
        return Task.CompletedTask;
    }

    public string Topic => FactTopicAtt.Get<TFactPayload>();


    private async Task FactReceived(object sender, BasicDeliverEventArgs ea)
    {
        try
        {
            Guard.Against.Null(ea, nameof(ea));
            Guard.Against.Null(ea.Body, nameof(ea.Body));
            var fact = _msg2Fact(ea.Body.ToArray());
            await Cast(fact);
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
        }
    }
}