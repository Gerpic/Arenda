using System;
using System.Collections.Generic;
using System.Linq;
using Arenda.Models;

namespace Arenda.Services
{
    public class BookingReportService
    {
        private readonly List<Booking> _bookings;

        public BookingReportService(List<Booking> bookings)
        {
            _bookings = bookings;
        }

        public List<BookingReportEntry> GetReport(DateTime from, DateTime to)
        {
            // Property и User подгружаются через навигационные свойства Booking.Property и Booking.User
            return _bookings
                .Where(b => b.StartDate >= from && b.EndDate <= to)
                .Select(b => new BookingReportEntry
                {
                    BookingId = b.Id,
                    PropertyName = b.Property != null ? $"{b.Property.Address}" : "—",
                    ClientName = b.User != null ? b.User.FullName : "—",
                    ManagerName = b.Property?.Owner?.FullName ?? "", // если нужно отображать владельца недвижимости как менеджера
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status,
                    Price = b.Property?.Price ?? 0
                })
                .ToList();
        }
    }
}