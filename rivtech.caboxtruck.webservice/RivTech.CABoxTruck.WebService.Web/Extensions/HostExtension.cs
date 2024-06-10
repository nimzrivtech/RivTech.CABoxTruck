using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Web;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Extensions
{
    public static class HostExtension
    {
        public static async Task<IHost> MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var environment = services.GetRequiredService<IWebHostEnvironment>();
                logger.LogInformation($"Starting Database Migration for {nameof(AppDbContext)}.");

                using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    //if (environment.IsDevelopment())
                    //{
                    //    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    //    if (pendingMigrations.Any())
                    //    {
                    //        logger.LogInformation($"Drop Database for {nameof(AppDbContext)}.");
                    //        await context.Database.EnsureDeletedAsync();
                    //    }
                    //}

                    //logger.LogInformation($"Migrate Database for {nameof(AppDbContext)}.");
                    //await context.Database.MigrateAsync();
                    //logger.LogInformation($"Seeding Data in {nameof(AppDbContext)}.");
                    //await AppDbContextSeed.Initialize(context);
                    //logger.LogInformation($"Done Seeding Data in {nameof(AppDbContext)}.");
                    //await AppDbContextSeed.SeedDataSets(context);
                    //await AppDbContextData.SeedDataSets(context);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, $"An error occurred migrating the Database for {nameof(AppDbContext)}.");
                    throw;
                }
            }

            return host;
        }
    }
}
