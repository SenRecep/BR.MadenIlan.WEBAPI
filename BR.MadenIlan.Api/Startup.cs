using System.Linq;

using BR.MadenIlan.Api.Models;
using BR.MadenIlan.Shared.ExtensionMethods;
using BR.MadenIlan.Shared.Filters;

using FluentValidation.AspNetCore;

using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BR.MadenIlan.Api
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(OPT =>
            {
                OPT.UseSqlServer(Configuration.GetConnectionString(Environment.GetConnectionType()));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.Authority = Environment.GetIdentityServerUrl(Configuration);
                    opt.Audience = "resource_product_api";
                    opt.RequireHttpsMetadata = !Environment.IsDevelopment();
                });


            services.AddOData();

            services.AddControllers(opt =>
            {
                opt.Filters.Add<ValidateModelAttribute>();

            }).AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<Startup>();
            });


            //services.AddSwaggerGen(c => 
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BR.MadenIlan.Api", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDelayRequestDevelopment();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BR.MadenIlan.Api v1"));
            }

            app.UseCustomExceptionHandler();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            ODataConventionModelBuilder oDataBuilder = new();
            oDataBuilder.EntitySet<Product>("Products");
            oDataBuilder.EntitySet<Category>("Categories");

            app.UseEndpoints(endpoints =>
            {
                endpoints.Select().Expand().OrderBy().Count().Filter();
                endpoints.MapODataRoute("odata", "odata", oDataBuilder.GetEdmModel());
                endpoints.MapControllers();
            });
        }
    }
}
