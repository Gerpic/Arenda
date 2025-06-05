using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
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

        [Column("comment")]
        public string Comment { get; set; }
    }
}