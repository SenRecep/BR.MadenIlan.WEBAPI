using BR.MadenIlan.Auth.Data;
using BR.MadenIlan.Auth.Models;
using BR.MadenIlan.Auth.Services;
using BR.MadenIlan.Shared.ExtensionMethods;

using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BR.MadenIlan.Auth
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalApiAuthentication();

            services.AddControllersWithViews().AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddCustomValidationResponse();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(Environment.GetConnectionType())));


            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            IIdentityServerBuilder builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;
            })
                .AddConfigurationStore(opt =>
                    opt.ConfigureDbContext = cOpt =>
                        cOpt.UseSqlServer(Configuration.GetConnectionString(Environment.GetConnectionType()), sqlOpt =>
                            sqlOpt.MigrationsAssembly("BR.MadenIlan.Auth")
                        )
                )
                .AddOperationalStore(opt =>
                    opt.ConfigureDbContext = cOpt =>
                        cOpt.UseSqlServer(Configuration.GetConnectionString(Environment.GetConnectionType()), sqlOpt =>
                            sqlOpt.MigrationsAssembly("BR.MadenIlan.Auth")
                        )
                )
                .AddAspNetIdentity<ApplicationUser>();
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiResources(Config.ApiResources)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)

            builder.AddDeveloperSigningCredential().AddResourceOwnerValidator<IdentityResourceOwnerPasswordValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseDelayRequestDevelopment();
            }
            app.UseCustomExceptionHandler();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}