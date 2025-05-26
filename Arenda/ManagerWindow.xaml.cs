using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Arenda.Data;
using Microsoft.EntityFrameworkCore;

namespace Arenda
{
    public partial class ManagerWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public ManagerWindow(int userId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            await LoadBookingsAsync();
        }

        private async System.Threading.Tasks.Task LoadBookingsAsync()
        {
            try
            {
                // Загрузка списка броней с основной инфой
                var bookings = await _dbContext.Bookings
                    .Include(b => b.Property)
                        .ThenInclude(p => p.Category)
                    .Include(b => b.Property)
                        .ThenInclude(p => p.City)
                    .OrderByDescending(b => b.BookingDate)
                    .Select(b => new BookingListItem
                    {
                        Id = b.Id,
                        PropertyCategory = b.Property.Category.CategoryName,
                        PropertyAddress = b.Property.Address,
                        City = b.Property.City.Name,
                        BookingDate = b.BookingDate
                    })
                    .ToListAsync();

                BookingsListView.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки броней: {ex.Message}");
            }
        }

        private void BookingsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingsListView.SelectedItem is BookingListItem booking)
            {
                var bookingDetailsWindow = new BookingDetailsWindow(booking.Id);
                bookingDetailsWindow.Owner = this;
                bookingDetailsWindow.ShowDialog();
                BookingsListView.SelectedItem = null;
            }
        }

        private void OpenChatsButton_Click(object sender, RoutedEventArgs e)
        {
            var chatListWindow = new ChatListWindow();
            chatListWindow.Owner = this;
            chatListWindow.ShowDialog();
        }
    }

    // Класс для представления элемента списка бронирований
    public class BookingListItem
    {
        public int Id { get; set; }
        public string PropertyCategory { get; set; }
        public string PropertyAddress { get; set; }
        public string City { get; set; }
        public DateTime BookingDate { get; set; }
    }
}