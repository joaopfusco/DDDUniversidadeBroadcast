using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Domain.Models
{
    public class Seguidor : BaseModel
    {
        public int SegueId { get; set; }

        public int SeguidoId { get; set; }

        [JsonIgnore]
        public Usuario Segue { get; set; }

        [JsonIgnore]
        public Usuario Seguido { get; set; }
    }
}
