using DDDUniversidadeBroadcast.Domain.DTOs;
using DDDUniversidadeBroadcast.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Data;

namespace DDDUniversidadeBroadcast.Infra.Repositories
{
    public class EventoRepository(AppDbContext db) : BaseRepository<Evento>(db), IEventoRepository
    {
    }
}
