
using BR.MadenIlan.Web.Shared.Models;
using BR.MadenIlan.WebUi.Managers;
using BR.MadenIlan.WebUi.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BR.MadenIlan.WebUi
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

            services.AddHttpClient();
            #region MyRegion
            //services.AddAuthentication(opts =>
            //{
            //    opts.DefaultScheme = "Cookies";
            //    opts.DefaultChallengeScheme = "oidc";
            //}).AddCookie("Cookies", opts =>
            //{
            //    opts.AccessDeniedPath = "/Home/AccessDenied";
            //}).AddOpenIdConnect("oidc", opts => 
            //{
            //    opts.SignInScheme = "Cookies";
            //    //opts.Authority = "https://madenilan.identityserver.senrecep.com";
            //    opts.Authority = "http://localhost:4456";
            //    opts.RequireHttpsMetadata = false;
            //    opts.ClientId = "WebClient_ROP";
            //    opts.ClientSecret = "madenilan_mobile_client_secret";
            //    opts.SignedOutRedirectUri = "http://localhost:44373/Home/Privacy";
            //    opts.ResponseType = "code id_token";
            //    opts.SaveTokens = true;
            //    opts.GetClaimsFromUserInfoEndpoint = true;
            //}); 
            #endregion
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
