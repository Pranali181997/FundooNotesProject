using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.FundooNoteContex
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options)
           : base(options)
        {
        }
        public DbSet<Entity.User> Users { get; set; }
        public DbSet<Note> Note { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Entity.User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).RegisterdDate = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
}