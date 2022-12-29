using EDM.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EDM.Data
{
    public class ApiDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;


        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
    }
}