using DDDUniversidadeBroadcast.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQ.Subscriber
{
    public class GetPostagem
    {
        public static async Task<Postagem> Get(int postagemId)
        {
            using var httpClient = new HttpClient();
            var url = $"http://localhost:5000/api/postagem/getPostagem/{postagemId}";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var postagem = JsonConvert.DeserializeObject<Postagem>(jsonString);

                if (postagem == null)
                {
                    throw new Exception($"Erro ao desserializar a postagem {postagemId}: o resultado é nulo.");
                }

                return postagem ?? throw new Exception($"Postagem {postagemId} nao encontrada.");
            }
            else
            {
                throw new Exception($"Erro ao buscar postagem {postagemId}: {response.StatusCode}");
            }
        }
    }
}
