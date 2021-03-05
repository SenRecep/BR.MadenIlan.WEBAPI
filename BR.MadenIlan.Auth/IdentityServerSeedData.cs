using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BR.MadenIlan.Auth.Data;
using BR.MadenIlan.Auth.ExtensionMethods;
using BR.MadenIlan.Auth.Models;
using BR.MadenIlan.Core.StringInfo;

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BR.MadenIlan.Auth
{
    public static class IdentityServerSeedData
    {

        public static async Task SeedUserData(IServiceProvider serviceProvider)
        {
            await SeedRoles(serviceProvider);
            await SeedUsers(serviceProvider);
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (IdentityRole role in DefaultUsersAndRoles.GetRoles())
            {
                var found = await roleManager.FindByNameAsync(role.Name);
                if (found != null) continue;

                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
            }
        }


        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (SignUpViewModel model in DefaultUsersAndRoles.GetDevelopers())
            {
                var found = await userManager.FindByNameAsync(model.UserName);
                if (found != null) continue;

                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result= await userManager.AddToRoleAsync(user,RoleInfo.Developer);

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
            }
        }

        public static async Task SeedConfiguration(ConfigurationDbContext context)
        {
            List<Task> tasks = new List<Task>();

            if (context.Clients.Count() != Config.Clients.Count())
            {
                await context.Clients.Clear();
                tasks.Add(context.Clients.AddRangeAsync(Config.Clients.Select(x => x.ToEntity())));
            }
            if (context.ApiResources.Count() != Config.ApiResources.Count())
            {
                await context.ApiResources.Clear();
                tasks.Add(context.ApiResources.AddRangeAsync(Config.ApiResources.Select(x => x.ToEntity())));
            }

            if (context.ApiScopes.Count() != Config.ApiScopes.Count())
            {
                await context.ApiScopes.Clear();
                tasks.Add(context.ApiScopes.AddRangeAsync(Config.ApiScopes.Select(x => x.ToEntity())));
            }

            if (context.IdentityResources.Count() != Config.IdentityResources.Count())
            {
                await context.IdentityResources.Clear();
                tasks.Add(context.IdentityResources.AddRangeAsync(Config.IdentityResources.Select(x => x.ToEntity())));
            }

            await Task.WhenAll(tasks);
            await context.SaveChangesAsync();
        }
    }
}
