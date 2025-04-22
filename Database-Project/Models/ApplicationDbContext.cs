using Microsoft.EntityFrameworkCore;
using Database_Project.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Database_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<BookStock> BookStocks { get; set; }
        public DbSet<LibraryBranch> LibraryBranches { get; set; }
        public DbSet<UnwantedCustomer> UnwantedCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnwantedCustomer>()
                .HasOne(uc => uc.User)
                .WithOne(u => u.UnwantedCustomer)
                .HasForeignKey<UnwantedCustomer>(uc => uc.UserId);
        }
    }
}