using Libary.Infastructure.EntityConfiguration;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infastructure.Context
{
    public class LibraryDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "LIB";

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        }
    }
}
