using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Neo4jClient;
using RestApi.Models;
using ServiceStack.Redis;

namespace RestApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string host2 = "pc.mokhnatkin.org";
            string user = "db_cw";
            string password = "LimonTr3";

            // Mongo
            services.AddSingleton<IMongoClient>(_ => new MongoClient());
            BsonClassMap.RegisterClassMap<Unit>();
            // Redis
            services.AddScoped<IRedisClientsManager>(_ => new RedisManagerPool());
            // Cassandra
            services.AddSingleton<ICluster>(_ =>
                Cluster.Builder()
                .AddContactPoint(host2)
                .WithPort(100)
                .WithCredentials(user, password)
                .Build());
            // Neo4j
            services.AddScoped<IGraphClient>(_ =>
            {
                var graphClient = new GraphClient(new Uri($"http://{host2}:105/db/data"), "db_cw", password);
                graphClient.Connect();
                return graphClient;
            });
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
