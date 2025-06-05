using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("residential_properties")]
    public class ResidentialProperty
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("owner_id")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }
        public PropertyCategory Category { get; set; }

        [Column("city_id")]
        public int? CityId { get; set; }
        public City City { get; set; }

        [Column("area")]
        public double Area { get; set; }

        [Column("room_count")]
        public int RoomCount { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }

        public ICollection<PropertyPhoto> Photos { get; set; } = new List<PropertyPhoto>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}