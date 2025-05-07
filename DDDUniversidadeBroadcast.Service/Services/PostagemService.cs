using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Publisher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Service.Services
{
    public class PostagemService(IPostagemRepository repository, IEventoService eventoService, IUsuarioService usuarioService) : BaseService<Postagem>(repository), IPostagemService
    {
        public override async Task<int> Insert(Postagem model)
        {
            var result = await base.Insert(model);
            await Publisher.PublishAsync(model.Id.ToString());
            return result;
        }
    }
}
