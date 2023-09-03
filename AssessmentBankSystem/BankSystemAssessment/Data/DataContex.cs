using Microsoft.EntityFrameworkCore;
using BankSystemAssessment.Model;


namespace BankSystemAssessment.Data
{
    public class DataContex : DbContext
    {
        public DataContex(DbContextOptions option) : base(option)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(18,4)");
        }
    }
}
