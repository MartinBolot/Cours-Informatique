﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TP3.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using TP3.Models.Repository;
using TP3.Models.DataManager;

namespace TP3
{
    public class Startup
    {
        /// <summary>
        /// startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Configure Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "TD1 API Documentation",
                    Version = "v1",
                });
                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(
                    typeof(Startup).Assembly.Location,
                    ".xml"
                );
                c.IncludeXmlComments(xmlFile);
            });

            //var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";
            var connection = "Server=localhost\\SQLEXPRESS; Database=Films;Trusted_Connection=True;";
            services.AddDbContext<FilmsContext>
                (options => options.UseSqlServer(connection));

            // dependency injection
            services.AddScoped<IDataRepository<Compte>, CompteManager>();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TD3 API Documentation");
            });
        }
    }
}
