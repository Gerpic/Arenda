using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arenda.Models
{
    [Table("property_listing_requests")]
    public class PropertyListingRequest
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("submitted_by")]
        public int SubmittedBy { get; set; }
        [ForeignKey("SubmittedBy")]
        public User SubmittedByUser { get; set; }

        [Column("submission_date")]
        public DateTime SubmissionDate { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("processed_by")]
        public int? ProcessedBy { get; set; }
        [ForeignKey("ProcessedBy")]
        public User ProcessedByUser { get; set; }

        [Column("processing_date")]
        public DateTime? ProcessingDate { get; set; }

        [Column("rejection_reason")]
        public string RejectionReason { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("area")]
        public decimal Area { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public PropertyCategory Category { get; set; }

        [Column("city_id")]
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }

        [Column("room_count")]
        public int RoomCount { get; set; }

        [Column("capacity")]
        public int Capacity { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        // Связанные фотографии
        public List<ListingRequestPhoto> Photos { get; set; }
    }
}