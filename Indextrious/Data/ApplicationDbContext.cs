using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Indextrious.Models;

namespace Indextrious.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CardFile> CardFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Self referencing relationship for CardFile
            modelBuilder.Entity<CardFile>()
                .HasMany(cf => cf.SubFiles)
                .WithOne()
                .HasForeignKey(cf => cf.Id); // Assuming CardFile has a foreign key Id

            // Relationship between ApplicationUser and CardFile
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserCollections)
                .WithOne(cf => cf.Owner)
                .HasForeignKey(cf => cf.OwnerId);
        }
    }
}
