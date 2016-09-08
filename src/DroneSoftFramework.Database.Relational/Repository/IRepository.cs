using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity;

namespace DroneSoftFramework.Database.Relational.Repository
{
    public interface IRepository<TDbContext, TEntity> 
        where TEntity : class, new()
        where TDbContext : DbContext, new()
    {
        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<IList<TEntity>> InsetManyAsync(IList<TEntity> entities);

        Task<TEntity> ModifyAsync(TEntity entity);

        Task<IList<TEntity>> ModifyManyAsync(IList<TEntity> entities);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> DeleteManyAsync(IList<TEntity> entities);
    }
}
