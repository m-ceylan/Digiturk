using Digiturk.Core.Entity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Digiturk.Core.Enums.Enums;

namespace Digiturk.Repository
{
    public class BaseRepository<T> where T : BaseEntity
    {
        private IMongoCollection<T> mongoCollection;
        private MongoClient client;
        string database;
        string collection = "";
        public BaseRepository(string connectionString, string database, string collection)
        {
            client = new MongoClient(connectionString);
            this.database = database;
            this.collection = collection;

            var db = client.GetDatabase(database);
            mongoCollection = db.GetCollection<T>(collection);
        }

        public MongoClient Client() => client;
        public IMongoCollection<T> Collection() => mongoCollection;
        public IMongoQueryable<T> GetAll()
        {
            return mongoCollection.AsQueryable().Where(x => x.RegistrationStatus != RegistrationStatus.Deleted);
        }
        public IMongoQueryable<T> GetBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return GetAll().Where(expression).Where(x => x.RegistrationStatus != RegistrationStatus.Deleted);
        }
        public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RegistrationStatus != RegistrationStatus.Deleted).AnyAsync();
        }
        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RegistrationStatus != RegistrationStatus.Deleted).CountAsync();
        }
        public async Task<T> GetByIdAsync(string Id)
        {
            return await mongoCollection.Find(x => x.Id == Id && x.RegistrationStatus != RegistrationStatus.Deleted).FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultByAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await GetAll().Where(expression).Where(x => x.RegistrationStatus != RegistrationStatus.Deleted).FirstOrDefaultAsync();
        }
        public virtual async Task<T> AddAsync(T entity, bool forceDates = false)
        {
            if (!forceDates)
            {
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
            }

            entity.RegistrationStatus = Core.Enums.Enums.RegistrationStatus.Active;
            await mongoCollection.InsertOneAsync(entity);
            return entity;
        }
    
        public virtual async Task UpdateAsync(T entity, bool forceDates = false)
        {
            if (!forceDates)
                entity.UpdateDate = DateTime.UtcNow;

            await mongoCollection.ReplaceOneAsync(m => m.Id == entity.Id, entity);
        }
        public virtual async Task UpdateAsync(string id, T entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            await mongoCollection.ReplaceOneAsync(m => m.Id == id, entity);
        }
        public virtual async Task DeleteAsync(T entity)
        {
            entity.RegistrationStatus = Core.Enums.Enums.RegistrationStatus.Deleted;
            entity.UpdateDate = DateTime.UtcNow;
            await this.UpdateAsync(entity.Id, entity);
        }
        public virtual async Task DeleteAsync(string id)
        {
            var entity = await this.GetByIdAsync(id);
            await this.DeleteAsync(entity);
        }
        public virtual async Task<DeleteResult> HardDeleteAsync(string id)
        {
            return await this.Collection().DeleteOneAsync(Builders<T>.Filter.Eq(x => x.Id, id));
        }
        public virtual async Task<DeleteResult> HardDeleteManyAsync(FilterDefinition<T> filter)
        {
            return await this.Collection().DeleteManyAsync(filter);
        }
        public virtual async Task<UpdateResult> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, bool updateDate = true)
        {
            if (updateDate)
                update = update.Set(x => x.UpdateDate, DateTime.UtcNow);

            return await mongoCollection.UpdateManyAsync(filter, update);
        }
        public virtual async Task<UpdateResult> DeleteManyAsync(FilterDefinition<T> filter)
        {
            var update = Builders<T>.Update
                .Set(x => x.RegistrationStatus, RegistrationStatus.Deleted)
                .Set(x => x.UpdateDate, DateTime.UtcNow);

            return await mongoCollection.UpdateManyAsync(filter, update);
        }

    }
}
