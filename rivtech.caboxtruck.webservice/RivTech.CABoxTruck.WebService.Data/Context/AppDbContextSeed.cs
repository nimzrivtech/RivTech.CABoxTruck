using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Context
{
    /// <summary>
    /// The seed class for the application.
    /// </summary>
    public static class AppDbContextSeed
    {
        /// <summary>
        /// Initializes the database to contain it's required entities.
        /// </summary>
        public static async Task Initialize(AppDbContext context)
        {
            context.ChangeTracker.DetectChanges();
            await context.SaveChangesAsync();
        }

        public static async Task SeedDataSets(AppDbContext context)
        {
            
        }

    }
}
