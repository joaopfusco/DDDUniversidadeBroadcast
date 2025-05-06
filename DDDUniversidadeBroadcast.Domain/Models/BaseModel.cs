using DDDUniversidadeBroadcast.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace DDDUniversidadeBroadcast.Domain.Models
{
    public class BaseModel : IBaseModel<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
