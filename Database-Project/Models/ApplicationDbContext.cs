using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Database_Project.Models;

namespace Database_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<
        User,                     // Custom User with int key
        IdentityRole<int>,        // IdentityRole with int key
        int,                      // Primary key type
        IdentityUserClaim<int>,
        IdentityUserRole<int>,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<BookStock> BookStocks { get; set; }
        public DbSet<LibraryBranch> LibraryBranches { get; set; }
        public DbSet<UnwantedCustomer> UnwantedCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UnwantedCustomer>()
                .HasOne(uc => uc.User)
                .WithOne(u => u.UnwantedCustomer)
                .HasForeignKey<UnwantedCustomer>(uc => uc.UserId);
        }
    }
}
