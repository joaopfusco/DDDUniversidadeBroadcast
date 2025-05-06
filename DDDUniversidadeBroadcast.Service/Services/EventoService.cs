using DDDUniversidadeBroadcast.Domain.Models;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Service.Services
{
    public class EventoService(IEventoRepository repository) : BaseService<Evento>(repository), IEventoService
    {
        public bool Lotado(int eventoId) => Get(eventoId).FirstOrDefault()?.Inscritos >= 50;
    }
}
