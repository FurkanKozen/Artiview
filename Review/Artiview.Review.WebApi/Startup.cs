using Artiview.Review.Application.Adapters;
using Artiview.Review.Application.Repositories;
using Artiview.Review.Infrastructure.Adapters;
using Artiview.Review.Infrastructure.DataAccess;
using Artiview.Review.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Artiview.Review.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStringModel = Configuration.GetSection("DbConnectionStrings:Review").Get<ConnectionStringModel>();

            SqlConnectionStringBuilder sqlConnectionStringBuilder = new()
            {
                DataSource = connectionStringModel.Server,
                InitialCatalog = connectionStringModel.Database,
                UserID = connectionStringModel.User,
                Password = connectionStringModel.Password,
                TrustServerCertificate = true,
            };
            var connectionString = sqlConnectionStringBuilder.ToString();
            services.AddDbContext<ReviewDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IApiAdapter, ApiAdapter>();
            services.AddSingleton((s) =>
            {
                var baseAddress = new Uri(Configuration["ApiAdapterConfigs:ArticleServiceBaseAddress"]);
                return new HttpClient()
                {
                    BaseAddress = baseAddress
                };
            });

            services.AddControllers()
                .AddOData(options => options.EnableQueryFeatures(100));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Artiview.Review.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artiview.Review.WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
