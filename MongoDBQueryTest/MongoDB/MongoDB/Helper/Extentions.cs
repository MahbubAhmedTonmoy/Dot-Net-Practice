using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Helper
{
    public static class Extentions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection("mongo"));
            services.AddSingleton(sp =>
            {
                var options = sp.GetService<IOptions<MongoOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });
            services.AddSingleton(sp =>
            {
                var options = sp.GetService<IOptions<MongoOptions>>();
                var client = sp.GetService<MongoClient>();
                return client.GetDatabase(options.Value.Database);
            });
        }
    }

    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }

    }
}
