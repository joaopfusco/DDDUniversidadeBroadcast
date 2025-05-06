using System.Text.Json.Serialization;

namespace DDDUniversidadeBroadcast.Domain.Models
{
    public class Usuario : BaseModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Curso { get; set; }

        [JsonIgnore]
        public ICollection<Seguidor> Seguidores { get; set; }

        [JsonIgnore]
        public ICollection<Seguidor> Seguindo { get; set; }

        [JsonIgnore]
        public ICollection<Postagem> Postagens { get; set; }
    }
}
