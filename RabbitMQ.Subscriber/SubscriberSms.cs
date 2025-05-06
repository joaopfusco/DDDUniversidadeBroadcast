using DDDUniversidadeBroadcast.Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Subscriber;

public class SubscriberSms
{
    public static async Task SubscribeAsync(List<Participante> participantes)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        string exchangeName = "meu_exchange_fanout";
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: "fanout"
        );

        string queueName = "minha-fila-sms";

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
            var texto = Encoding.UTF8.GetString(body);
            
            try
            {
                await SendSms(participantes, texto);
                Console.WriteLine("Mensagem processada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
            }

            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
            consumer: consumer
        );

        Console.WriteLine("Aguardando mensagens.");
    }

    private static async Task SendSms(List<Participante> participantes, string texto) 
    {
        using var httpClient = new HttpClient();

        foreach (var participante in participantes)
        {
            var telefone = participante.Usuario.Telefone;
            Console.WriteLine($"Enviando sms para {telefone} com o texto: {texto}");
            await Task.Delay(500);
            Console.WriteLine($"Sms enviado para {telefone} com sucesso!");
        }
    }
}
