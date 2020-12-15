using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace MongoDB.Repository
{
    public interface IRepository
    {
        string DatabaseName { get; set; }

        void Delete<T>(Expression<Func<T, bool>> dataFilters);
        void Delete<T>(Expression<Func<T, bool>> dataFilters, string collectionName);
        string ExecuteCommand(string query);
        T GetItem<T>(Expression<Func<T, bool>> dataFilters);
        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters);
        IQueryable<T> GetItems<T>();
        void Initialize();
        void Save<T>(T data, string collectionName = "");
        void Save<T>(List<T> datas);
        void Update<T>(Expression<Func<T, bool>> dataFilters, T data);
        void UpdateMany<T>(Expression<Func<T, bool>> dataFilters, object data, string collectionName = "");
    }
    public class MongoRepository : IRepository
    {
        private readonly string connectionString;
        public string DatabaseName
        {
            get { return "MongoDbQuery"; }
            set { }
        }
        public string TenantId { get { return "MongoDbQuery"; } }
        public IMongoDatabase DataContext { get; set; }

        public MongoRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("");
        }

        public void Initialize()
        {
            DataContext = new MongoClient(connectionString).GetDatabase(DatabaseName);
        }

        public void Delete<T>(Expression<Func<T, bool>> dataFilters)
        {
            Initialize();
            DataContext.GetCollection<T>(typeof(T).Name + "s").DeleteMany(dataFilters);
        }

        public void Delete<T>(Expression<Func<T, bool>> dataFilters, string collectionName)
        {
            Initialize();
            DataContext.GetCollection<T>(collectionName + "s").DeleteMany(dataFilters);
        }

        public string ExecuteCommand(string query)
        {
            Initialize();
            BsonDocument obj = DataContext.RunCommand(new BsonDocumentCommand<BsonDocument>(BsonDocument.Parse(query)));
            return obj.ToJson();
        }

        public T GetItem<T>(Expression<Func<T, bool>> dataFilters)
        {
            Initialize();
            return DataContext.GetCollection<T>(typeof(T).Name + "s").AsQueryable().FirstOrDefault(dataFilters);
        }

        public IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters)
        {
            Initialize();
            return DataContext.GetCollection<T>(typeof(T).Name + "s").AsQueryable().Where(dataFilters);
        }

        public IQueryable<T> GetItems<T>()
        {
            Initialize();
            return DataContext.GetCollection<T>(typeof(T).Name + "s").AsQueryable();
        }

        public void Save<T>(T data, string collectionName = "")
        {
            Initialize();
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(T).Name + "s";
            }
            DataContext.GetCollection<T>(collectionName).InsertOne(data);
        }

        public void Save<T>(List<T> datas)
        {
            Initialize();
           
            DataContext.GetCollection<T>(typeof(T).Name + "s").InsertMany(datas);
        }

        public void Update<T>(Expression<Func<T, bool>> dataFilters, T data)
        {
            Initialize();
            DataContext.GetCollection<T>(typeof(T).Name + "s").ReplaceOne(dataFilters, data);
        }

        public void UpdateMany<T>(Expression<Func<T, bool>> dataFilters, object data, string collectionName = "")
        {
            IDictionary<string, object> values = GetValues(data);
            UpdateDefinition<T> updateDefinition = null;
            foreach (KeyValuePair<string, object> item in values)
            {
                updateDefinition = ((updateDefinition != null) ? ((!(item.Value is string[])) ? updateDefinition.Set(item.Key, item.Value) : updateDefinition.Set(item.Key, (string[])item.Value)) : ((!(item.Value is string[])) ? Builders<T>.Update.Set(item.Key, item.Value) : Builders<T>.Update.Set(item.Key, (string[])item.Value)));
            }
            Initialize();
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(T).Name + "s";
            }
            DataContext.GetCollection<T>(collectionName).UpdateMany(dataFilters, updateDefinition);
        }
        private IDictionary<string, object> GetValues(object obj)
        {
            return obj.GetType().GetProperties().ToDictionary((PropertyInfo p) => p.Name, (PropertyInfo p) => (p.GetValue(obj) == null) ? null : p.GetValue(obj));
        }
    }
}
