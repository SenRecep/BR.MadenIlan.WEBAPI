using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace BR.MadenIlan.Auth
{
    public static class IdentityServerSeedData
    {
        public static async Task Seed(ConfigurationDbContext context)
        {
            var tasks = new List<Task>();

            if (!context.Clients.Any())
                tasks.Add(context.Clients.AddRangeAsync(Config.Clients.Select(x => x.ToEntity())));

            if (!context.ApiResources.Any())
                tasks.Add(context.ApiResources.AddRangeAsync(Config.ApiResources.Select(x => x.ToEntity())));

            if (!context.ApiScopes.Any())
                tasks.Add(context.ApiScopes.AddRangeAsync(Config.ApiScopes.Select(x => x.ToEntity())));

            if (!context.IdentityResources.Any())
                tasks.Add(context.IdentityResources.AddRangeAsync(Config.IdentityResources.Select(x => x.ToEntity())));

            await Task.WhenAll(tasks);
            await context.SaveChangesAsync();
        }
    }
}
