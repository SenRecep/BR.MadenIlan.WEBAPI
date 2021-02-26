
using BR.MadenIlan.AuthBootLoader.Managers;
using BR.MadenIlan.AuthBootLoader.Mapping.AutoMapperProfile;
using BR.MadenIlan.AuthBootLoader.Services;
using BR.MadenIlan.Web.Shared.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BR.MadenIlan.AuthBootLoader
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
            services.Configure<ApiClient>(Configuration.GetSection("ApiClient"));

            services.AddScoped<ITokenService, TokenManager>();
            services.AddScoped<IAuthService, AuthManager>();

            #region Mappers

            services.AddAutoMapper(typeof(TokenProfile));
            #endregion

            services.AddHttpClient();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BR.MadenIlan.AuthBootLoader", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BR.MadenIlan.AuthBootLoader v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
