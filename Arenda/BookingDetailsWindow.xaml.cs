using System.Windows;
using Arenda.Models;
using Arenda.Data;
using Microsoft.EntityFrameworkCore;

namespace Arenda.Windows
{
    public partial class BookingDetailsWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private Booking _booking;

        public BookingDetailsWindow(int bookingId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            LoadBooking(bookingId);
        }

        private async void LoadBooking(int bookingId)
        {
            // Загружаем все нужные данные сразу
            _booking = await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Property)
                    .ThenInclude(p => p.Category)
                .Include(b => b.Property)
                    .ThenInclude(p => p.City)
                .Include(b => b.Property)
                    .ThenInclude(p => p.Owner)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (_booking != null)
            {
                DataContext = _booking;
            }
            else
            {
                MessageBox.Show("Бронирование не найдено.");
                Close();
            }
        }

        private void UserChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_booking?.User != null)
            {
                var chatWindow = new ChatWindow(_booking.User.Id);
                chatWindow.Owner = this;
                chatWindow.ShowDialog();
            }
        }

        private void OwnerChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_booking?.Property?.Owner != null)
            {
                var chatWindow = new ChatWindow(_booking.Property.Owner.Id);
                chatWindow.Owner = this;
                chatWindow.ShowDialog();
            }
        }
    }
}