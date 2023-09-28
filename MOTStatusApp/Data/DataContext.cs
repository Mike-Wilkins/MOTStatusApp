using Microsoft.EntityFrameworkCore;
using MOTStatusWebApi.Data;

namespace MOTStatusWebApi.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
           
        }
        public DbSet<MOTStatusDetails> MOTStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MOTStatusDetails>().
                HasKey(msd => msd.Id);
        }
    }
}
