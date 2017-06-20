using System;
using Cassandra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Neo4jClient;
using ServiceStack.Redis;
using Swashbuckle.AspNetCore.Swagger;
using RestApi.Model;
using MySQL.Data.EntityFrameworkCore.Extensions;
using RestApi.Middleware;

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
            string host2 = "35.189.70.205";
            string user = "db_cw";
            string password = "LimonTr3";

           
            //MySql
            services.AddDbContext<MySqlContext>(options => options.UseMySQL("Server=35.189.70.205;Database=db;Uid=root;Pwd=root;"));
            // Redis
            services.AddScoped<IRedisClientsManager>(_ => new RedisManagerPool());
            // Cassandra
            services.AddSingleton<ICluster>(_ =>
                Cluster.Builder()
                .AddContactPoint(host2)
                .WithPort(9042)
                .WithCredentials("cassandra", "cassandra")
                .Build());
            services.AddSingleton<IStaffDriver, DbDriver>();
            // Add framework services.
            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Rest API", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}
