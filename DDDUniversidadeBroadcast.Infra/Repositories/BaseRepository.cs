using DDDUniversidadeBroadcast.Domain.Models;
using System.Linq.Expressions;
using DDDUniversidadeBroadcast.Infra.Interfaces;
using DDDUniversidadeBroadcast.Infra.Data;

namespace DDDUniversidadeBroadcast.Infra.Repositories
{
    public class BaseRepository<TModel>(AppDbContext db) : IBaseRepository<TModel> where TModel : BaseModel
    {
        protected readonly AppDbContext _db = db;

        public IQueryable<TModel> Get(Expression<Func<TModel, bool>>? predicate = null)
        {
            var query = _db.Set<TModel>().AsQueryable();
            if (predicate != null) query = query.Where(predicate);
            return query;
        }

        public IQueryable<TModel> Get(int id)
        {
            return Get(p => p.Id.Equals(id))
                .AsQueryable();
        }

        public virtual int Insert(TModel model)
        {
            try
            {
                _db.Add(model);
                return _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual int Update(TModel model)
        {
            try
            {
                _db.Update(model);
                return _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual int Delete(int id)
        {
            try
            {
                TModel model = Get(id).FirstOrDefault() ?? throw new Exception("Entity not found!");
                _db.Remove(model);
                return _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
