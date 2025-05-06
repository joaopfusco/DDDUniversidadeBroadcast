using DDDUniversidadeBroadcast.Service.Interfaces;
using DDDUniversidadeBroadcast.Domain.Models;
using System.Linq.Expressions;
using DDDUniversidadeBroadcast.Infra.Data;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Repositories;

namespace DDDUniversidadeBroadcast.Service.Services
{
    public class BaseService<TModel>(IBaseRepository<TModel> repository) : IBaseService<TModel> where TModel : BaseModel
    {
        protected readonly IBaseRepository<TModel> _repostitory = repository;

        public IQueryable<TModel> Get(Expression<Func<TModel, bool>>? predicate = null)
        {
            return _repostitory.Get(predicate);
        }

        public IQueryable<TModel> Get(int id)
        {
            return _repostitory.Get(id);
        }

        public virtual async Task<int> Insert(TModel model)
        {
            return await _repostitory.Insert(model);
        }   

        public virtual async Task<int> Update(TModel model)
        {
            return await _repostitory.Update(model);
        }

        public virtual async Task<int> Delete(int id)
        {
            return await _repostitory.Delete(id);
        }
    }
}
