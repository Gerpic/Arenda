using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    public class RealEstate
    {
         public int Id { get; set; }
         public string Address { get; set; }
         public string Category { get; set; }
         public decimal Area { get; set; }
         public decimal Price { get; set; }
         public int Rooms { get; set; }
         public string[] Photos { get; set; }
    }

    [Table("roles")]
    public class Role
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("role_name")]
        public string RoleName { get; set; }
    }

    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Column("is_owner")]
        public bool IsOwner { get; set; }

        public ICollection<ResidentialProperty> Properties { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    [Table("property_categories")]
    public class PropertyCategory
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("category_name")]
        public string CategoryName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public ICollection<ResidentialProperty> Properties { get; set; }
    }

    [Table("cities")]
    public class City
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public ICollection<ResidentialProperty> Properties { get; set; }
    }

    [Table("residential_properties")]
    public class ResidentialProperty
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("area")]
        public decimal Area { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }
        public PropertyCategory Category { get; set; }

        [Column("city_id")]
        public int CityId { get; set; }
        public City City { get; set; }

        [Column("room_count")]
        public int RoomCount { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        public ICollection<PropertyPhoto> Photos { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }


    [Table("reviews")]
    public class Review
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("property_id")]
        public int PropertyId { get; set; }
        public ResidentialProperty Property { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("review_date")]
        public DateTime ReviewDate { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [Column("comment")]
        public string Comment { get; set; }
    }

    [Table("property_photos")]
    public class PropertyPhoto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("property_id")]
        public int PropertyId { get; set; }
        public ResidentialProperty Property { get; set; }

        [Column("photo_url")]
        public string PhotoUrl { get; set; }

        [Column("upload_date")]
        public DateTime UploadDate { get; set; }
    }
}
