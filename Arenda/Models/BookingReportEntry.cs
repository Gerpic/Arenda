using System;

namespace Arenda.Models
{
    public class BookingReportEntry
    {
        public int BookingId { get; set; }
        public string PropertyName { get; set; }
        public string ClientName { get; set; }
        public string ManagerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}