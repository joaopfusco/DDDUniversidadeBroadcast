﻿using DDDUniversidadeBroadcast.Domain.DTOs;
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
using Microsoft.AspNetCore.Mvc;

namespace DDDUniversidadeBroadcast.Infra.Repositories
{
    public class SeguidorRepository(AppDbContext db) : BaseRepository<Seguidor>(db), ISeguidorRepository
    {
    }
}
