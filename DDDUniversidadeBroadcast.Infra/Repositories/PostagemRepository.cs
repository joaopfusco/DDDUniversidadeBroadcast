using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Data;
using RabbitMQ.Publisher;
using RabbitMQ.Subscriber;

namespace DDDUniversidadeBroadcast.Infra.Repositories
{
    public class PostagemRepository(AppDbContext db) : BaseRepository<Postagem>(db), IPostagemRepository
    {
        public override int Insert(Postagem model)
        {
            var publisher = new Publisher();
            _ = publisher.PublishAsync().GetAwaiter();

            var subscriber = new Subscriber();
            _ = subscriber.SubscribeAsync().GetAwaiter();

            return base.Insert(model);
        }
    }
}
