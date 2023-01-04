using Ardalis.GuardClauses;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace DotCart.Drivers.RabbitMQ;

public interface IRmqListenerDriver<TFactPayload, TFactMeta>
    : IListenerDriverT<TFactPayload, TFactMeta>
    where TFactPayload : IPayload
    where TFactMeta : class
{
}

public class RMqListenerDriverT<TFactPayload, TFactMeta>
    : DriverB, IListenerDriverT<TFactPayload, byte[]>
    where TFactPayload : IPayload
    where TFactMeta : IEventMeta
{
    private readonly IConnectionFactory _connFact;
    private readonly Msg2Fact<TFactPayload, TFactMeta, byte[]> _msg2Fact;


    private IModel _channel;
    private IConnection _connection;
    private AsyncEventingBasicConsumer _consumer;
    private string _topic;

    public RMqListenerDriverT(
        IConnectionFactory connFact,
        Msg2Fact<TFactPayload, TFactMeta, byte[]> msg2Fact)
    {
        _connFact = connFact;
        _msg2Fact = msg2Fact;
    }

    public override void Dispose()
    {
        _connection.Dispose();
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
            Log.Debug($"{AppFacts.Received} Fact({FactTopicAtt.Get<TFactPayload>()})");
            Cast(fact);
        }
        catch (Exception e)
        {
            Log.Error(e.InnerAndOuter());
        }
    }
}