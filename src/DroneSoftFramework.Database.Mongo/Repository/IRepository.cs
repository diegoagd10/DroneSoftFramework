using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using DroneSoftFramework.Database.Mongo.Entity;

namespace DroneSoftFramework.Database.Mongo.Repository
{
    public interface IRepository<TEntity> where TEntity : DroneEntityBase, new()
    {
        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<IList<TEntity>> InsetManyAsync(IList<TEntity> entities);

        Task<TEntity> ModifyAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity);

        Task<IList<TEntity>> ModifyManyAsync(Expression<Func<TEntity, bool>> predicate, IList<TEntity> entities);

        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
