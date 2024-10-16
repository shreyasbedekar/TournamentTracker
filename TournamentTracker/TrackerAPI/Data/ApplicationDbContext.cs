using Microsoft.EntityFrameworkCore;
using TrackerLibrary.Models;

namespace TrackerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PrizeModel> prizes { get; set; }
        public DbSet<TeamModel> teams { get; set; }
        public DbSet<PersonModel> people { get; set; }

    }
}
