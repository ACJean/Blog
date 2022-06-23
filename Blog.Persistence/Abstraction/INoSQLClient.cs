using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lavel.STD.Entities.NoSQL
{
    public interface INoSQLClient
    {
        public Collection Insert<Collection>(Collection data);
        public Collection Insert<Collection>(Collection data, NoSqlDbType db);
        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter);
        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter, SortDefinition<Collection> sort);
        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter, SortDefinition<Collection> sort, NoSqlDbType db);
        public List<Collection> Get<Collection>(FilterDefinition<Collection> filter, SortDefinition<Collection> sort, NoSqlDbType db, int? page, int? pageSize);
        public bool Delete<Collection>(FilterDefinition<Collection> filter);
        public bool Delete<Collection>(FilterDefinition<Collection> filter, NoSqlDbType db);
        public bool Update<Collection>(FilterDefinition<Collection> filter, Collection data);
        public bool Update<Collection>(FilterDefinition<Collection> filter, Collection data, NoSqlDbType db);
    }

    public enum NoSqlDbType
    {
        blog_service
    }
}
