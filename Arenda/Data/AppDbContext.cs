using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Arenda.Models;

namespace Arenda.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ResidentialProperty> ResidentialProperties { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PropertyPhoto> PropertyPhotos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Username=postgres;Password=12345;Database=Arenda");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.Properties)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Properties)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(r => r.City)
                .WithMany(c => c.Properties)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropertyPhoto>()
                .HasOne(p => p.Property)
                .WithMany(r => r.Photos)
                .HasForeignKey(p => p.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Property)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
