using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Polly.Retry;
using RabbitMQ.Client;
using Serilog;

namespace DotCart.Drivers.RabbitMQ;

public class RMqEmitterDriverT<TIFact, TPayload> 
    : DriverB, IEmitterDriverT<TPayload, byte[]>
    where TIFact : IFactB
    where TPayload: IPayload
{
    private readonly int _backoff = 100;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly IConnectionFactory _connFact;
    private readonly Fact2Msg<TPayload, byte[]> _fact2Msg;
    private readonly int _maxRetries = Polly.Config.MaxRetries;


    public RMqEmitterDriverT(
        IConnectionFactory connFact,
        Fact2Msg<TPayload, byte[]> fact2Msg)
    {
        _connFact = connFact;
        _fact2Msg = fact2Msg;
        _connection = _connFact.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(TopicAtt.Get<TIFact>(), ExchangeType.Fanout);
    }


    public string Topic => TopicAtt.Get<TIFact>();

    public Task EmitAsync(FactT<TPayload> fact, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            Log.Information($"{AppVerbs.Emitting} Fact[{TopicAtt.Get(fact)}]");
            var body = _fact2Msg(fact);
            _channel.BasicPublish(Topic, "", null, body);
            return Task.CompletedTask;
        }, cancellationToken);
    }

    public override void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
        base.Dispose();
    }

    
}