using AutoMapper;
using Dapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xero.Products.Api.Configuration;
using Xero.Products.Api.Resources;
using Xero.Products.Api.Validation;
using Xero.Products.BusinessLayer;
using Xero.Products.Repository;

namespace Xero.Products.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddFluentValidation();

            // Services
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddScoped<IProductService, ProductService>();

            // Validators
            services.AddSingleton<IValidator<CreateUpdateProductResource>, CreateUpdateProductResourceValidator>();
            services.AddSingleton<IValidator<CreateUpdateProductOptionResource>, CreateUpdateProductOptionResourceValidator>();

            // Config
            services.AddScoped<IAppConfig, AppConfig>();
            services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfig"));

            // Mapping
            services.AddAutoMapper(this.GetType().Assembly);

            // Register type handlers for dapper
            SqlMapper.AddTypeHandler(new DapperGuidTypeHandler());
            SqlMapper.AddTypeHandler(new DapperDecimalTypeHandler());

            // Bad Request Response Middleware
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
                    var result = new
                    {
                        Code = "00009",
                        Message = "Validation errors",
                        Errors = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}