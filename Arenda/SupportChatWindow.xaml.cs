using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Arenda.Data;
using Arenda.Models;

namespace Arenda.Windows
{
    public partial class SupportChatWindow : Window, INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private int? SelectedTicketId = null;
        private int _adminId;

        public ObservableCollection<TicketViewModel> Tickets { get; set; }

        private TicketViewModel _selectedTicket;
        public TicketViewModel SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                if (_selectedTicket != value)
                {
                    _selectedTicket = value;
                    OnPropertyChanged(nameof(SelectedTicket));
                    if (_selectedTicket != null)
                    {
                        SelectedTicketId = _selectedTicket.TicketId;
                        LoadMessages(_selectedTicket.TicketId);
                    }
                }
            }
        }

        private ObservableCollection<SupportMessageViewModel> _currentMessages;
        public ObservableCollection<SupportMessageViewModel> CurrentMessages
        {
            get => _currentMessages;
            set
            {
                _currentMessages = value;
                OnPropertyChanged(nameof(CurrentMessages));
            }
        }

        public SupportChatWindow(int adminId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _adminId = adminId;
            this.DataContext = this;
            LoadTickets();
        }

        private void LoadTickets()
        {
            Tickets = new ObservableCollection<TicketViewModel>(
                _dbContext.SupportTicket
                    .Include(t => t.User)
                    .OrderByDescending(t =>
                        _dbContext.SupportMessage
                            .Where(m => m.TicketId == t.Id)
                            .OrderByDescending(m => m.SentAt)
                            .Select(m => (DateTime?)m.SentAt)
                            .FirstOrDefault() ?? t.CreatedAt
                    )
                    .Select(t => new TicketViewModel
                    {
                        TicketId = t.Id,
                        UserFullName = t.User != null ? t.User.FullName : $"Пользователь {t.UserId}",
                        UserId = t.User != null ? t.User.Id : t.UserId,
                        LastMessageTime = _dbContext.SupportMessage
                            .Where(m => m.TicketId == t.Id)
                            .OrderByDescending(m => m.SentAt)
                            .Select(m => (DateTime?)m.SentAt)
                            .FirstOrDefault() ?? t.CreatedAt
                    }).ToList()
            );
            OnPropertyChanged(nameof(Tickets));
            if (Tickets.Count > 0)
                SelectedTicket = Tickets[0];
        }

        private void LoadMessages(int ticketId)
        {
            var ticket = _dbContext.SupportTicket.Include(t => t.User).FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null) return;

            var messages = new ObservableCollection<SupportMessageViewModel>();

            // Первое сообщение пользователя (обращение)
            messages.Add(new SupportMessageViewModel
            {
                Text = ticket.Message,
                Time = ticket.CreatedAt,
                IsFromSupport = false,
                Alignment = HorizontalAlignment.Left,
                BubbleColor = "#f1f5f9"
            });

            // Все остальные сообщения по тикету
            var replies = _dbContext.SupportMessage
                .Where(m => m.TicketId == ticketId)
                .OrderBy(m => m.SentAt)
                .ToList();

            foreach (var msg in replies)
            {
                messages.Add(new SupportMessageViewModel
                {
                    Text = msg.Text,
                    Time = msg.SentAt,
                    IsFromSupport = msg.IsFromSupport,
                    Alignment = msg.IsFromSupport ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                    BubbleColor = msg.IsFromSupport ? "#e3f1ff" : "#f1f5f9"
                });
            }

            CurrentMessages = messages;
        }

        private void DialogList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DialogList.SelectedItem is TicketViewModel ticket)
            {
                SelectedTicket = ticket;
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTicketId == null || string.IsNullOrWhiteSpace(MessageTextBox.Text))
                return;

            var text = MessageTextBox.Text.Trim();
            var message = new SupportMessage
            {
                TicketId = SelectedTicketId.Value,
                SenderId = _adminId,
                Text = text,
                SentAt = DateTime.Now,
                IsFromSupport = true
            };
            _dbContext.SupportMessage.Add(message);
            _dbContext.SaveChanges();

            CurrentMessages.Add(new SupportMessageViewModel
            {
                Text = message.Text,
                Time = message.SentAt,
                IsFromSupport = true,
                Alignment = HorizontalAlignment.Right,
                BubbleColor = "#e3f1ff"
            });

            MessageTextBox.Text = "";
        }

        // Логика для кнопки назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var adminWindow = new AdminWindow(_adminId);
            adminWindow.Show();
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // ViewModel для тикета
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        public string UserFullName { get; set; }
        public int UserId { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public string UserFullNameAndId => $"{UserFullName} (ID: {UserId})";
    }

    // ViewModel для сообщений
    public class SupportMessageViewModel
    {
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool IsFromSupport { get; set; }
        public HorizontalAlignment Alignment { get; set; }
        public string BubbleColor { get; set; }
    }
}