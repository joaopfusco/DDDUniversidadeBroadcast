using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Domain.Models
{
    public class Evento : BaseModel
    {
        public string Nome { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public bool Aberto { get; set; }

        [NotMapped]
        public int Inscritos
        {
            get
            {
                return Participantes?.Count ?? 0;
            }
        }

        [JsonIgnore]
        public ICollection<Participante> Participantes { get; set; }

        [JsonIgnore]
        public ICollection<Postagem> Postagens { get; set; }
    }
}
