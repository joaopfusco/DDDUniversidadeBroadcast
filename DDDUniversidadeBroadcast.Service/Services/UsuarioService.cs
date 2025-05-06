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
    public class UsuarioService(IUsuarioRepository repository) : BaseService<Usuario>(repository), IUsuarioService
    {

    }
}
