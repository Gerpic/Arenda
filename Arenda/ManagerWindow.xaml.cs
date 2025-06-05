using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Arenda.Data;
using Microsoft.EntityFrameworkCore;
using Arenda.Windows;

namespace Arenda
{
    public partial class ManagerWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly int _userId;

        public ManagerWindow(int userId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _userId = userId;
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
                var bookingDetailsWindow = new BookingDetailsWindow(booking.Id, _userId);
                bookingDetailsWindow.Show();
                this.Close();
            }
        }

        private void OpenChatsButton_Click(object sender, RoutedEventArgs e)
        {
            var chatWindow = new ChatWindow(_userId);
            chatWindow.Show();
            this.Close();
        }

        private void OpenListingRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ListingRequestsWindow(_userId);
            window.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Открытие окна авторизации (или главное окно приложения)
            var loginWindow = new LogInWindow();
            loginWindow.Show();
            this.Close();
        }
    }

    public class BookingListItem
    {
        public int Id { get; set; }
        public string PropertyCategory { get; set; }
        public string PropertyAddress { get; set; }
        public string City { get; set; }
        public DateTime BookingDate { get; set; }
    }
}