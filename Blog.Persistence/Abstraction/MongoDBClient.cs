using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lavel.STD.Entities.NoSQL
{
    public class MongoDBClient : INoSQLClient
    {

        protected MongoClient client;

        public MongoDBClient(MongoClient mongoClient)
        {
            client = mongoClient;
        }

        protected IMongoDatabase GetMongoDatabase(string dbName)
        {
            return client.GetDatabase(dbName.ToString());
        } 

        protected IMongoCollection<Collection> GetCollection<Collection>(IMongoDatabase database)
        {
            return database.GetCollection<Collection>(typeof(Collection).Name);
        }

        public Collection Insert<Collection>(Collection data, NoSqlDbType db) 
        {
            try
            {
                var database = GetMongoDatabase(db.ToString());
                var collectionDB = GetCollection<Collection>(database);
                collectionDB.InsertOne(data);
            }
            catch (Exception ex)
            {
                //log
            }
            return data;
        }

        public Collection Insert<Collection>(Collection data)
        {
            return Insert(data, NoSqlDbType.blog_service);
        }

        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter)
        {
            return Get(filter, null);
        }

        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter, SortDefinition<Collection> sort)
        {
            return Get(filter, sort, NoSqlDbType.blog_service);
        }

        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter, SortDefinition<Collection> sort, NoSqlDbType db)
        {
            return Get(filter, sort, db, null, null);
        }

        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter, SortDefinition<Collection> sort, NoSqlDbType db, int? page, int? pageSize)
        {
            List<Collection> response = default;
            try
            {
                int? skip = page != null && pageSize != null ? (page - 1) * pageSize : null;
                var database = GetMongoDatabase(db.ToString());
                var collectionDB = GetCollection<Collection>(database);

                if (sort != null && skip != null) response = collectionDB.Find(filter).Sort(sort).Skip(skip).Limit(pageSize).ToList();
                else if (sort != null) response = collectionDB.Find(filter).Sort(sort).ToList();
                else response = collectionDB.Find(filter).ToList();

            }
            catch (Exception ex)
            {
                //log
            }
            return response;
        }

        public bool Update<Collection>(FilterDefinition<Collection> filter, Collection data, NoSqlDbType db)
        {
            bool response = false;
            try
            {
                var database = GetMongoDatabase(db.ToString());
                var collectionDB = GetCollection<Collection>(database);
                ReplaceOneResult replaceOneResult = collectionDB.ReplaceOne(filter, data);
                if (replaceOneResult.ModifiedCount > 0) response = true;
            }
            catch (Exception ex)
            {
                //log
            }
            return response;
        }

        public bool Update<Collection>(FilterDefinition<Collection> filter, Collection data)
        {
            return Update(filter, data, NoSqlDbType.blog_service);
        }

        public bool Delete<Collection>(FilterDefinition<Collection> filter, NoSqlDbType db)
        {
            bool response = false;
            try
            {
                var database = GetMongoDatabase(db.ToString());
                var collectionDB = GetCollection<Collection>(database);
                DeleteResult deleteResult = collectionDB.DeleteOne(filter);
                if (deleteResult.DeletedCount > 0) response = true;
            }
            catch (Exception ex)
            {
                //log
            }
            return response;
        }

        public bool Delete<Collection>(FilterDefinition<Collection> filter)
        {
            return Delete(filter, NoSqlDbType.blog_service);
        }
    }
}
