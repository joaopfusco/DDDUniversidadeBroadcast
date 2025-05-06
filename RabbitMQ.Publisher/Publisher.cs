using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Publisher;

public class Publisher
{
    public async Task PublishAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var entity = new
        {
            Nome = "Palestra de Tecnologia 8",
            Local = "Auditório Principal",
            Descricao = "Uma palestra sobre as tendências da tecnologia.",
            DataHora = DateTime.Now.AddDays(7),
            Aberto = true,
        };
        string entityJson = JsonSerializer.Serialize(entity);
        var entityBody = Encoding.UTF8.GetBytes(entityJson);

        string exchangeName = "meu_exchange_fanout";
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: "fanout"
        );

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: "",
            mandatory: false,
            body: entityBody
        );

        Console.WriteLine("Mensagem enviada!");
    }
}
