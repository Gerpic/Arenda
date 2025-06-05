using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using Arenda.Data;
using Arenda.Models;
using Arenda.ViewModels;

namespace Arenda.Windows
{
    public partial class BookingChatWindow : Window
    {
        private int _chatId;
        private int _currentUserId;
        private int _interlocutorId;
        private int _bookingId;
        private int _managerId;
        public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();
        public string InterlocutorName { get; set; }

        public BookingChatWindow(int chatId, int currentUserId, int interlocutorId, int bookingId, int managerId)
        {
            InitializeComponent();
            _chatId = chatId;
            _currentUserId = currentUserId;
            _interlocutorId = interlocutorId;
            _bookingId = bookingId;
            _managerId = managerId;
            SetInterlocutorName();
            DataContext = this;
            LoadMessages();
        }

        private void SetInterlocutorName()
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == _interlocutorId);
                InterlocutorName = user != null ? user.FullName : $"ID: {_interlocutorId}";
            }
        }

        private void LoadMessages()
        {
            using (var db = new AppDbContext())
            {
                var msgs = db.Messages
                    .Where(m => m.ChatId == _chatId)
                    .OrderBy(m => m.Time)
                    .ToList();

                Messages.Clear();
                foreach (var msg in msgs)
                {
                    Messages.Add(new MessageViewModel
                    {
                        Content = msg.Content,
                        Time = msg.Time,
                        IsOwn = msg.SenderId == _currentUserId
                    });
                }
            }

            // Прокручиваем вниз после обновления сообщений
            this.Dispatcher.InvokeAsync(() =>
            {
                ChatScrollViewer?.ScrollToEnd();
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string text = MessageTextBox.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            using (var db = new AppDbContext())
            {
                var msg = new Message
                {
                    ChatId = _chatId,
                    SenderId = _currentUserId,
                    Content = text,
                    Time = DateTime.Now,
                    IsRead = false
                };
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            MessageTextBox.Clear();
            LoadMessages();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var bookingDetailsWindow = new BookingDetailsWindow(_bookingId, _managerId);
            bookingDetailsWindow.Show();
            this.Close();
        }
    }
}