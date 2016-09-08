using System;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Globalization;
using System.Configuration;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using DroneSoftFramework.Database.Mongo.Entity;

namespace DroneSoftFramework.Database.Mongo.Repository
{
    internal class DroneMongoRepository<TEntity> : IRepository<TEntity> where TEntity : DroneEntityBase, new()
    {
        private IMongoQueryable<TEntity> _queryable;
        private IMongoCollection<TEntity> _collection;
        
        public DroneMongoRepository()
        {
            string connectionString = ConfigurationManager.AppSettings["MongoDbConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("MongoDbConnectionString");

            string databaseName = ConfigurationManager.AppSettings["MongoDbName"];
            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentNullException("MongoDbName");

            _collection = new MongoClient(connectionString)
                .GetDatabase(databaseName)
                    .GetCollection<TEntity>(PluralizationService.CreateService(CultureInfo.CurrentUICulture).Pluralize(typeof(TEntity).Name));

            _queryable = _collection.AsQueryable();
        }

        #region IRepository

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            DeleteResult result = await _collection.DeleteOneAsync<TEntity>(predicate);
            return result.DeletedCount > 0;
        }

        public async Task<bool> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            DeleteResult result = await _collection.DeleteManyAsync<TEntity>(predicate);
            return result.DeletedCount > 0;
        }

        public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _queryable.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _queryable.FirstOrDefaultAsync<TEntity>(predicate);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IList<TEntity>> InsetManyAsync(IList<TEntity> entities)
        {
            foreach (var e in entities)
                e.Id = Guid.NewGuid();

            await _collection.InsertManyAsync(entities);
            return entities;
        }

        public async Task<TEntity> ModifyAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            if (entity.Id == null)
                return await InsertAsync(entity);

            ReplaceOneResult result = await _collection.ReplaceOneAsync(predicate, entity, new UpdateOptions { IsUpsert = true });
            if (result.ModifiedCount > 0)
                return entity;
            return null;
        }

        public async Task<IList<TEntity>> ModifyManyAsync(Expression<Func<TEntity, bool>> predicate, IList<TEntity> entities)
        {
            IList<TEntity> result = new List<TEntity>();

            foreach (var entity in entities)
            {
                var entityResult = await ModifyAsync(predicate, entity);
                if (entityResult != null) result.Add(entityResult); 
            }
            return result;
        }

        #endregion
    }
}
