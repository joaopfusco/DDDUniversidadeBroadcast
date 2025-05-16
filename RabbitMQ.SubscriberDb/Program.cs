using RabbitMQ.Client.Events;
using RabbitMQ.Subscriber;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.SubscriberDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var subscriber = new SubscriberClass("minha-fila-db", async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var postagemId = int.Parse(Encoding.UTF8.GetString(body));
                await PutParticipantes(postagemId);
            }, dlqEnabled: true);
            subscriber.Start();

            Console.WriteLine("Pressione Enter para sair...");
            Console.ReadLine();
        }

        private static async Task PutParticipantes(int postagemId)
        {
            try
            {
                using var httpClient = new HttpClient();

                var postagem = await GetPostagem.Get(postagemId);

                foreach (var participante in postagem.Evento.Participantes)
                {
                    var texto = $"Nova postagem de {postagem.Autor.Nome} no evento {postagem.Evento.Nome}. Conteudo: {postagem.Conteudo}";
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
                        throw new Exception($"Erro ao atualizar participante {participante.Id}: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
