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
using Microsoft.Extensions.Logging;

namespace DDDUniversidadeBroadcast.API.Controllers
{
    public class UsuarioController(IUsuarioService service, ILogger<UsuarioController> logger) : BaseController<Usuario>(service, logger)
    {
    }
}
