using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Subscriber;

public class Subscriber
{
    public async Task SubscribeAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        string exchangeName = "meu_exchange_fanout";
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: "fanout"
        );

        string queueName = "minha-fila";

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        await channel.QueueBindAsync(
            queue: queueName,
            routingKey: "",
            exchange: exchangeName
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            int code = 0;

            var entity = JsonSerializer.Deserialize<dynamic>(json);
            if (entity != null)
            {
                Console.WriteLine("Recebido ENTIDADE. Enviando para a API...");
                
            }

            Console.WriteLine("Mensagem processada com erro!");

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
            consumer: consumer
        );

        Console.WriteLine("Aguardando mensagens. Pressione [enter] para sair.");
        Console.ReadLine();
    }
}
