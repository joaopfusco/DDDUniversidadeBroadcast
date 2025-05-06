using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Data;
using RabbitMQ.Publisher;
using RabbitMQ.Subscriber;

namespace DDDUniversidadeBroadcast.Infra.Repositories
{
    public class PostagemRepository(AppDbContext db) : BaseRepository<Postagem>(db), IPostagemRepository
    {
    }
}
