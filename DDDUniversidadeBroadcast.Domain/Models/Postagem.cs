using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Domain.Models
{
    public class Postagem : BaseModel
    {
        public int AutorId { get; set; }

        public int EventoId { get; set; }

        public string Conteudo { get; set; }
        public DateTime DataHora { get; set; }
        public int Curtidas { get; set; }
        public int Comentarios { get; set; }

        [JsonIgnore]
        public Usuario Autor { get; set; }

        [JsonIgnore]
        public Evento Evento { get; set; }
    }
}
