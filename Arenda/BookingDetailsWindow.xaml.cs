using System.Windows;
using Arenda.Models;
using Arenda.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Arenda.Windows
{
    public partial class BookingDetailsWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private Booking _booking;
        private int _managerId;

        public BookingDetailsWindow(int bookingId, int managerId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _managerId = managerId;
            LoadBooking(bookingId);
        }

        private async void LoadBooking(int bookingId)
        {
            _booking = await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Property).ThenInclude(p => p.Owner)
                .Include(b => b.Property).ThenInclude(p => p.Category)
                .Include(b => b.Property).ThenInclude(p => p.City)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (_booking != null)
            {
                DataContext = _booking;
                UpdateActionButtons();
            }
            else
            {
                MessageBox.Show("Бронирование не найдено.");
                Close();
            }
        }

        // Кнопка "Чат с пользователем"
        private void UserChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_booking?.User != null && _booking?.Property != null)
            {
                var chat = EnsureChatExists(_booking.User.Id, _booking.Property.Id, _managerId);
                int currentUserId = _managerId;
                int interlocutorId = _booking.User.Id;
                var chatWindow = new BookingChatWindow(chat.Id, currentUserId, interlocutorId, _booking.Id, _managerId);
                chatWindow.Owner = this;
                chatWindow.Show();
                this.Hide();
            }
        }

        // Кнопка "Чат с владельцем"
        private void OwnerChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (_booking?.Property?.Owner != null && _booking?.Property != null)
            {
                var chat = EnsureChatExists(_booking.Property.Owner.Id, _booking.Property.Id, _managerId);
                int currentUserId = _managerId;
                int interlocutorId = _booking.Property.Owner.Id;
                var chatWindow = new BookingChatWindow(chat.Id, currentUserId, interlocutorId, _booking.Id, _managerId);
                chatWindow.Owner = this;
                chatWindow.Show();
                this.Hide();
            }
        }

        // Ищет или создает чат
        // Теперь учитываем managerId и корректно его устанавливаем
        private Chat EnsureChatExists(int guestId, int propertyId, int managerId)
        {
            using (var context = new AppDbContext())
            {
                var chat = context.Chats
                    .FirstOrDefault(c => c.GuestId == guestId && c.PropertyId == propertyId && c.ManagerId == managerId);

                if (chat == null)
                {
                    // Проверка, что managerId существует в users
                    var managerExists = context.Users.Any(u => u.Id == managerId);
                    if (!managerExists)
                    {
                        MessageBox.Show("Ошибка: Менеджер с таким ID не найден в базе пользователей!");
                        throw new System.Exception("ManagerId не найден в users");
                    }

                    chat = new Chat
                    {
                        GuestId = guestId,
                        PropertyId = propertyId,
                        ManagerId = managerId
                    };
                    context.Chats.Add(chat);
                    context.SaveChanges();
                }
                return chat;
            }
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null) return;
            _booking.Status = "Подтверждено";
            _dbContext.Bookings.Update(_booking);
            await _dbContext.SaveChangesAsync();
            DataContext = null;
            DataContext = _booking;
            MessageBox.Show("Бронирование подтверждено.");
            UpdateActionButtons();
        }

        private async void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_booking == null) return;
            _booking.Status = "Отменено";
            _dbContext.Bookings.Update(_booking);
            await _dbContext.SaveChangesAsync();
            DataContext = null;
            DataContext = _booking;
            MessageBox.Show("Бронирование отменено.");
            UpdateActionButtons();
        }

        private void UpdateActionButtons()
        {
            bool show = _booking != null && _booking.Status != null && _booking.Status == "Ожидает";
            AcceptButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            RejectButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var managerWindow = new ManagerWindow(_managerId);
            managerWindow.Show();
            this.Close();
        }
    }
}