using RabbitMQ.Client.Events;
using RabbitMQ.Subscriber;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ.SubscriberSms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var subscriber = new SubscriberClass("minha-fila-sms", async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var postagemId = int.Parse(Encoding.UTF8.GetString(body));
                await SendSms(postagemId);
            }, dlqEnabled: true);
            subscriber.Start();

            Console.WriteLine("Pressione Enter para sair...");
            Console.ReadLine();
        }

        private static async Task SendSms(int postagemId)
        {
            try
            {
                using var httpClient = new HttpClient();

                var postagem = await GetPostagem.Get(postagemId);

                foreach (var participante in postagem.Evento.Participantes)
                {
                    var texto = $"Nova postagem de {postagem.Autor.Nome} no evento {postagem.Evento.Nome}. Conteudo: {postagem.Conteudo}";
                    var telefone = participante.Usuario.Telefone;
                    Console.WriteLine($"Enviando sms para {telefone} com o texto: {texto}");
                    await Task.Delay(500);
                    Console.WriteLine($"Sms enviado para {telefone} com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
