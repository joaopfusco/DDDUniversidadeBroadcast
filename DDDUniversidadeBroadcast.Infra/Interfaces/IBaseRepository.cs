using DDDUniversidadeBroadcast.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DDDUniversidadeBroadcast.Infra.Interfaces
{
    public interface IBaseRepository<TModel> where TModel : BaseModel
    {
        IQueryable<TModel> Get(Expression<Func<TModel, bool>>? predicate = null);

        IQueryable<TModel> Get(int id);

        Task<int> Insert(TModel model);

        Task<int> Update(TModel model);

        Task<int> Delete(int id);
    }
}
