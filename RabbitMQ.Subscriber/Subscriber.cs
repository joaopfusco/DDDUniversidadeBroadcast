using DDDUniversidadeBroadcast.Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Subscriber;

public class Subscriber
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
            var texto = Encoding.UTF8.GetString(body);
            
            try
            {
                await PutParticipantes(participantes, texto);
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

        Console.WriteLine("Aguardando mensagens. Pressione [enter] para sair.");
        Console.ReadLine();
    }

    private static async Task PutParticipantes(List<Participante> participantes, string texto) 
    {
        using var httpClient = new HttpClient();

        foreach (var participante in participantes)
        {
            participante.UltimaNotificacao = texto;

            var url = $"http://localhost:5000/api/participante/{participante.Id}";
            var content = new StringContent(
                JsonSerializer.Serialize(participante),
                Encoding.UTF8,
                "application/json"
            );
            var response = await httpClient.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Participante {participante.Id} atualizado com sucesso.");
            }
            else
            {
                Console.WriteLine($"Erro ao atualizar participante {participante.Id}: {response.StatusCode}");
            }
        }
    }
}
