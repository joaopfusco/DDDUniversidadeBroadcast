using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Subscriber;

public class SubscriberSms(IServiceScopeFactory scopeFactory)
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
            var postagemId = Encoding.UTF8.GetString(body);
            
            try
            {
                await SendSms(postagemId);
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

    private async Task SendSms(string postagemId) 
    {
        using var httpClient = new HttpClient();

        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPostagemService>();

        var postagem = service.Get(int.Parse(postagemId))
            .Include(p => p.Autor)
            .Include(p => p.Evento)
            .ThenInclude(e => e.Participantes)
            .ThenInclude(p => p.Usuario)
            .FirstOrDefault() ?? throw new Exception($"Postagem {postagemId} nao encontrada.");

        foreach (var participante in postagem.Evento.Participantes)
        {
            var texto = $"Nova postagem de {postagem.Autor.Nome} no evento {postagem.Evento.Nome}. Conteudo: {postagem.Conteudo}";
            var telefone = participante.Usuario.Telefone;
            Console.WriteLine($"Enviando sms para {telefone} com o texto: {texto}");
            await Task.Delay(500);
            Console.WriteLine($"Sms enviado para {telefone} com sucesso!");
        }
    }
}
