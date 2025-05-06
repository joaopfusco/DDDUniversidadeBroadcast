using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Publisher;
using RabbitMQ.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Service.Services
{
    public class PostagemService(IPostagemRepository repository, IEventoService eventoService, IUsuarioService usuarioService) : BaseService<Postagem>(repository), IPostagemService
    {
        public override int Insert(Postagem model)
        {
            var usuario = usuarioService.Get(model.AutorId)
                .FirstOrDefault() ?? throw new Exception("User not found");

            var evento = eventoService.Get(model.EventoId)
                .Include(e => e.Participantes).FirstOrDefault() ?? throw new Exception("Event not found");

            var texto = $"Nova postagem de {usuario.Nome} no evento {evento.Nome}. Conteudo: {model.Conteudo}";
            Publisher.PublishAsync(texto).GetAwaiter().GetResult();

            var participantes = evento.Participantes.ToList();
            Subscriber.SubscribeAsync(participantes).GetAwaiter().GetResult();

            return base.Insert(model);
        }
    }
}
