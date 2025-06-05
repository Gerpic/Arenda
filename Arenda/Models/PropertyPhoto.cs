using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
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

        [Column("photo_url2")]
        public string PhotoUrl2 { get; set; }

        [Column("photo_url3")]
        public string PhotoUrl3 { get; set; }

        [Column("photo_url4")]
        public string PhotoUrl4 { get; set; }

        [Column("photo_url5")]
        public string PhotoUrl5 { get; set; }
    }
}