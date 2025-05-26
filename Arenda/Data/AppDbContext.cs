using Microsoft.EntityFrameworkCore;
using Arenda.Models;

namespace Arenda.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ResidentialProperty> ResidentialProperties { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PropertyPhoto> PropertyPhotos { get; set; }
        public DbSet<Booking> Bookings { get; set; } // Добавили DbSet для бронирований

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Arenda;Username=postgres;Password=12345");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(rp => rp.City)
                .WithMany(c => c.Properties)
                .HasForeignKey(rp => rp.CityId);

            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(rp => rp.Category)
                .WithMany(pc => pc.Properties)
                .HasForeignKey(rp => rp.CategoryId);

            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(rp => rp.Owner)
                .WithMany(u => u.Properties)
                .HasForeignKey(rp => rp.OwnerId);

            modelBuilder.Entity<PropertyPhoto>()
                .HasOne(p => p.Property)
                .WithMany(rp => rp.Photos)
                .HasForeignKey(p => p.PropertyId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Property)
                .WithMany(rp => rp.Reviews)
                .HasForeignKey(r => r.PropertyId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // Связи для таблицы бронирований
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany(rp => rp.Bookings)
                .HasForeignKey(b => b.PropertyId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);
        }
    }
}