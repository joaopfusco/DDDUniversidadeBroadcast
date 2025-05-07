using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Data;

namespace DDDUniversidadeBroadcast.Infra.Repositories
{
    public class PostagemRepository(AppDbContext db) : BaseRepository<Postagem>(db), IPostagemRepository
    {
    }
}
