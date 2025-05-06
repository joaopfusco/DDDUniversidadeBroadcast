using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Domain.Models
{
    public class Participante : BaseModel
    {
        public int EventoId { get; set; }

        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Evento Evento { get; set; }

        [JsonIgnore]
        public Usuario Usuario { get; set; }
    }
}
