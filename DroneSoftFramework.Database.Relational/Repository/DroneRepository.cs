using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DroneSoftFramework.Database.Relational.Factory;

namespace DroneSoftFramework.Database.Relational.Repository
{
    internal class DroneRepository<TDbContext, TEntity> : IRepository<TDbContext, TEntity>
        where TEntity : class, new()
        where TDbContext : DbContext, new()
    {
        #region IRepository

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            bool result = false;
            using (DbContext context = CreateContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entity == null) throw new ArgumentNullException("entity");

                        context.Set<TEntity>().Attach(entity);
                        context.Set<TEntity>().Remove(entity);

                        await context.SaveChangesAsync();

                        result = true;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public async Task<bool> DeleteManyAsync(IList<TEntity> entities)
        {
            bool result = false;
            using (DbContext context = CreateContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entities == null) throw new ArgumentNullException("entities");

                        foreach (var entity in entities)
                        {
                            context.Set<TEntity>().Attach(entity);
                            context.Set<TEntity>().Remove(entity);
                        }

                        await context.SaveChangesAsync();

                        result = true;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            IList<TEntity> result = default(IList<TEntity>);
            using (DbContext context = CreateContext())
            {
                try
                {
                    DbSet<TEntity> entities = context.Set<TEntity>();
                    result = await entities.Where(predicate).ToListAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity result = default(TEntity);
            using (DbContext context = CreateContext())
            {
                try
                {
                    DbSet<TEntity> entities = context.Set<TEntity>();
                    result = await entities.FirstOrDefaultAsync(predicate);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            TEntity result = default(TEntity);
            using (DbContext context = CreateContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entity == null) throw new ArgumentNullException("entity");

                        result = context.Set<TEntity>().Add(entity);

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public async Task<IList<TEntity>> InsetManyAsync(IList<TEntity> entities)
        {
            IList<TEntity> result = new List<TEntity>();
            using (DbContext context = CreateContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entities == null) throw new ArgumentNullException("entities");

                        foreach (var entity in entities)
                        {
                            result.Add(context.Set<TEntity>().Add(entity));
                        }

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public async Task<TEntity> ModifyAsync(TEntity entity)
        {
            TEntity result = default(TEntity);
            using (DbContext context = CreateContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entity == null) throw new ArgumentNullException("entity");

                        context.Set<TEntity>().Attach(entity);
                        context.Entry(entity).State = EntityState.Modified;

                        await context.SaveChangesAsync();

                        result = entity;
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public async Task<IList<TEntity>> ModifyManyAsync(IList<TEntity> entities)
        {
            IList<TEntity> result = new List<TEntity>();
            using (DbContext context = CreateContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (entities == null) throw new ArgumentNullException("entities");

                        foreach (var entity in entities)
                        {
                            context.Set<TEntity>().Attach(entity);
                            context.Entry(entity).State = EntityState.Modified;
                            result.Add(entity);
                        }

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Helpers

        private DbContext CreateContext()
        {
            return DbContextFactory.CreateDbContext<TDbContext>();
        }

        #endregion
    }
}
