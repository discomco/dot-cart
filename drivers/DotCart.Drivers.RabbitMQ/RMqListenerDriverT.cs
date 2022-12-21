using Ardalis.GuardClauses;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace DotCart.Drivers.RabbitMQ;

public abstract class RMqListenerDriverT<TFact> : DriverB, IListenerDriverT<TFact>
    where TFact : IFactB
{
    private readonly IConnectionFactory _connFact;
    private IModel _channel;
    private IConnection _connection;
    private AsyncEventingBasicConsumer _consumer;

    protected RMqListenerDriverT(IConnectionFactory connFact)
    {
        _connFact = connFact;
    }

    public string Topic => GetTopic();

    public abstract Task<TFact> CreateFactAsync(object source, CancellationToken cancellationToken = default);


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
        Log.Debug($"[{Topic}]-SUB [{GetType().PrettyPrint()}]");
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

    private string GetTopic()
    {
        return TopicAtt.Get(this);
    }

    private async Task FactReceived(object sender, BasicDeliverEventArgs ea)
    {
        try
        {
            Guard.Against.Null(ea, nameof(ea));
            Guard.Against.Null(ea.Body, nameof(ea.Body));
            var fact = await CreateFactAsync(ea.Body.ToArray());
            Log.Debug($"[{Topic}]-RCV Fact({TopicAtt.Get(fact)})");
            Cast(fact);
        }
        catch (Exception e)
        {
            Log.Error(e.InnerAndOuter());
        }
    }

    protected abstract IMsg CreateFact(byte[] data);
}