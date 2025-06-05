using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("listing_request_photos")]
    public class ListingRequestPhoto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("request_id")]
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public PropertyListingRequest Request { get; set; }

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