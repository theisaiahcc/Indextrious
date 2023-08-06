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

        public DbSet<CardCollection> CardCollections { get; set; }
        public DbSet<CardFile> CardFiles { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Self referencing relationship for CardFile
            modelBuilder.Entity<CardFile>()
                .HasMany(cf => cf.SubFiles)
                .WithOne(cf => cf.ParentCardFile)
                .HasForeignKey(cf => cf.ParentCardFileId)
                .OnDelete(DeleteBehavior.Restrict); // Deletion of CardFiles will have to be dealt with manually

            // Relationship between CardFile and CardCollection
            modelBuilder.Entity<CardFile>()
                .HasOne(cf => cf.ParentCollection)
                .WithMany(cc => cc.CardFiles)
                .HasForeignKey(cf => cf.ParentCollectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between ApplicationUser and CardCollection
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserCollections)
                .WithOne(cc => cc.Owner)
                .HasForeignKey(cc => cc.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Explicitly mention TPH inheritance
            modelBuilder.Entity<Card>()
                .HasDiscriminator<int>("CardType")
                .HasValue<Card>(1)
                .HasValue<IndexCard>(2);

            // Define relationship between Card and CardFile
            modelBuilder.Entity<Card>()
                .HasOne(c => c.CardFile)
                .WithMany(cf => cf.Cards)
                .HasForeignKey(c => c.CardFileId);
        }
    }
}
