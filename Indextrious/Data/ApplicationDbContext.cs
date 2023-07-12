﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Self referencing relationship for CardFile
            modelBuilder.Entity<CardFile>()
                .HasMany(cf => cf.SubFiles)
                .WithOne(cf => cf.ParentCardFile)
                .HasForeignKey(cf => cf.ParentCardFileId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust the delete behavior as needed

            // Relationship between CardFile and CardCollection
            modelBuilder.Entity<CardFile>()
                .HasOne(cf => cf.ParentCollection)
                .WithMany(cc => cc.CardFiles)
                .HasForeignKey(cf => cf.ParentCollectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship between ApplicationUser and CardCollection
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserCollections)
                .WithOne(cc => cc.Owner)
                .HasForeignKey(cc => cc.OwnerId)
                .IsRequired();
        }
    }
}
