using System.Net;
using DDDUniversidadeBroadcast.Domain.DTOs;
using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Publisher;

namespace DDDUniversidadeBroadcast.API.Controllers
{
    public class PostagemController(IPostagemService service, ILogger<PostagemController> logger) : BaseController<Postagem>(service, logger)
    {

        [HttpGet("[action]/{id}")]
        public IActionResult GetPostagem(int id)
        {
            return TryExecute(() =>
            {
                return Ok(service.Get(id)
                    .Include(p => p.Autor)
                    .Include(p => p.Evento)
                    .ThenInclude(e => e.Participantes)
                    .ThenInclude(pa => pa.Usuario)
                    .FirstOrDefault()
                );
            });
        }

        [HttpGet("[action]/{postagemId}")]
        public IActionResult SendMessage(string postagemId)
        {
            return TryExecute(() =>
            {
                Publisher.SendMessage(postagemId);
                return Ok();
            });
        }
    }
}
