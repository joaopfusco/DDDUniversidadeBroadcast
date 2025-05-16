using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Subscriber;

public class SubscriberClass
{
    private readonly string _host = "localhost";
    private readonly string _exchange = "meu_exchange_fanout";
    private readonly string _queueName;
    private readonly Func<object?, BasicDeliverEventArgs, Task> _onMessageCallback;
    private readonly bool _dlqEnabled;

    private IConnection _connection;
    private IModel _channel;

    public SubscriberClass(
        string queueName,
        Func<object?, BasicDeliverEventArgs, Task> onMessageCallback,
        bool dlqEnabled = false)
    {
        _queueName = queueName;
        _onMessageCallback = onMessageCallback;
        _dlqEnabled = dlqEnabled;
        Initialize();
    }

    private void Initialize()
    {
        var factory = new ConnectionFactory() { HostName = _host };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

        IDictionary<string, object>? args = null;

        if (_dlqEnabled)
        {
            const string dlxExchange = "dlx_exchange";
            const string dlqName = "minha-dlq";

            _channel.ExchangeDeclare(exchange: dlxExchange, type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: dlqName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: dlqName, exchange: dlxExchange, routingKey: "");

            args = new Dictionary<string, object>
            {
                { "x-message-ttl", 10000 },
                { "x-dead-letter-exchange", dlxExchange }
            };
        }

        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: args);
        _channel.QueueBind(queue: _queueName, exchange: _exchange, routingKey: "");
    }

    public void Start()
    {
        Console.WriteLine("Listening RabbitMQ on Port 5672...");

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, ea) =>
        {
            try
            {
                await _onMessageCallback(sender, ea);
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
    }
}
