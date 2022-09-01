using Microsoft.EntityFrameworkCore;
using WebFinancialHelper.Models;

namespace WebFinancialHelper.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<CollectedData> CollectedData { get; set; } 
    }
}
