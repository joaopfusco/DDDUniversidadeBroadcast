using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Domain.Interfaces
{
    public interface IBaseModel<T>
    {
        T Id { get; set; }
    }
}
