using RabbitMQ.Client.Events;
using RabbitMQ.Subscriber;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ.SubscriberDlq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var subscriber = new SubscriberClass("minha-fila-dlq", async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = int.Parse(Encoding.UTF8.GetString(body));
                await Callback(message);
            });
            subscriber.Start();

            Console.WriteLine("Pressione Enter para sair...");
            Console.ReadLine();
        }

        private static async Task Callback(int postagemId)
        {
            try
            {
                Console.WriteLine(postagemId);
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
