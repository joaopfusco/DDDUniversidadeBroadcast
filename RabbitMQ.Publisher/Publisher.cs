using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Publisher;

public class Publisher
{
    public static async Task PublishAsync(string texto)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var body = Encoding.UTF8.GetBytes(texto);

        string exchangeName = "meu_exchange_fanout";
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: "fanout"
        );

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: "",
            mandatory: false,
            body: body
        );

        Console.WriteLine("Mensagem enviada!");
    }
}
