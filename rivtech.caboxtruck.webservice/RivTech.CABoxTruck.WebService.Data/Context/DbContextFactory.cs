using Microsoft.EntityFrameworkCore;

namespace RivTech.CABoxTruck.WebService.Data.Context
{
    public class DbContextFactory
    {
        public AppDbContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new AppDbContext(options);
        }
    }
}
