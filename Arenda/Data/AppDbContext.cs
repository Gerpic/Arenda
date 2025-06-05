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
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PropertyListingRequest> PropertyListingRequests { get; set; }
        public DbSet<SupportTicket> SupportTicket { get; set; }
        public DbSet<SupportMessage> SupportMessage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Arenda;Username=postgres;Password=12345");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ResidentialProperty -> City
            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(rp => rp.City)
                .WithMany(c => c.Properties)
                .HasForeignKey(rp => rp.CityId);

            // ResidentialProperty -> Category
            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(rp => rp.Category)
                .WithMany(pc => pc.Properties)
                .HasForeignKey(rp => rp.CategoryId);

            // ResidentialProperty -> Owner (User)
            modelBuilder.Entity<ResidentialProperty>()
                .HasOne(rp => rp.Owner)
                .WithMany(u => u.Properties)
                .HasForeignKey(rp => rp.OwnerId);

            // PropertyPhoto -> Property
            modelBuilder.Entity<PropertyPhoto>()
                .HasOne(p => p.Property)
                .WithMany(rp => rp.Photos)
                .HasForeignKey(p => p.PropertyId);

            // Review -> Property
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Property)
                .WithMany(rp => rp.Reviews)
                .HasForeignKey(r => r.PropertyId);

            // Review -> User
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // Booking -> Property
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany(rp => rp.Bookings)
                .HasForeignKey(b => b.PropertyId);

            // Booking -> User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // Chat -> ResidentialProperty
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Property)
                .WithMany()
                .HasForeignKey(c => c.PropertyId);

            // Chat -> Guest (User)
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Guest)
                .WithMany()
                .HasForeignKey(c => c.GuestId);

            // Message -> Chat
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);

            // Message -> Sender (User)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId);
        }
    }
}