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
    public class PostagemService(IPostagemRepository repository) : BaseService<Postagem>(repository), IPostagemService
    {
        public void NotificarParticipantes()
        {
            // Implementar a lógica de notificação para os participantes
            // Isso pode envolver o uso de um serviço de mensageria ou outro mecanismo de notificação
        }
    }
}
