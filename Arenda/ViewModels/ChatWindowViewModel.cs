using System.Collections.ObjectModel;
using System.Linq;
using Arenda.Data;
using Arenda.Models;

namespace Arenda.ViewModels
{
    public class ChatWindowViewModel
    {
        public ObservableCollection<ChatListItemViewModel> ChatList { get; set; } = new ObservableCollection<ChatListItemViewModel>();
        public ChatListItemViewModel SelectedChat
        {
            get => _selectedChat;
            set
            {
                _selectedChat = value;
                if (_selectedChat != null)
                {
                    LoadMessages(_selectedChat.ChatId, _selectedChat.InterlocutorId);
                }
            }
        }
        private ChatListItemViewModel _selectedChat;

        private int _managerId;

        public ChatWindowViewModel(int managerId)
        {
            _managerId = managerId;
            LoadChatList();
        }

        public ChatWindowViewModel(int managerId, int chatId, int interlocutorId)
        {
            _managerId = managerId;
            LoadChatList();
            var chat = ChatList.FirstOrDefault(c => c.ChatId == chatId && c.InterlocutorId == interlocutorId);
            if (chat != null)
            {
                SelectedChat = chat;
                LoadMessages(chatId, interlocutorId);
            }
        }

        public void LoadChatList()
        {
            using (var db = new AppDbContext())
            {
                ChatList.Clear();
                var chats = db.Chats.ToList();

                foreach (var chat in chats)
                {
                    db.Entry(chat).Reference(c => c.Property).Load();
                    db.Entry(chat.Property).Reference(p => p.Owner).Load();
                    db.Entry(chat.Property).Reference(p => p.City).Load();

                    var property = chat.Property;
                    var owner = property?.Owner;
                    var guest = db.Users.FirstOrDefault(u => u.Id == chat.GuestId);
                    string city = property?.City?.Name ?? "";
                    string address = property?.Address ?? "";
                    string cityAndAddress = (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(address))
                        ? $"{city}, {address}"
                        : city + address;

                    int ownerId = property?.OwnerId ?? 0;
                    int guestId = chat.GuestId;

                    int interlocutorId;
                    string displayName;
                    string userRole;

                    // Определяем собеседника
                    if (_managerId == guestId)
                    {
                        interlocutorId = ownerId;
                        displayName = owner?.FullName ?? $"Пользователь {interlocutorId}";
                    }
                    else
                    {
                        interlocutorId = guestId;
                        displayName = guest?.FullName ?? $"Пользователь {interlocutorId}";
                    }

                    // Определяем роль собеседника
                    if (interlocutorId == ownerId)
                        userRole = "Владелец";
                    else
                        userRole = "Клиент";

                    var lastMsg = db.Messages
                        .Where(m => m.ChatId == chat.Id)
                        .OrderByDescending(m => m.Time)
                        .FirstOrDefault();

                    ChatList.Add(new ChatListItemViewModel
                    {
                        ChatId = chat.Id,
                        InterlocutorId = interlocutorId,
                        DisplayName = displayName,
                        LastMessage = lastMsg != null ? lastMsg.Content : "",
                        CityAndAddress = cityAndAddress,
                        UserRole = userRole,
                        Messages = new ObservableCollection<MessageViewModel>()
                    });
                }
            }
        }

        public void LoadMessages(int chatId, int interlocutorId)
        {
            var chat = ChatList.FirstOrDefault(c => c.ChatId == chatId && c.InterlocutorId == interlocutorId);
            if (chat == null) return;

            chat.Messages.Clear();
            using (var db = new AppDbContext())
            {
                var messages = db.Messages
                    .Where(m => m.ChatId == chatId)
                    .OrderBy(m => m.Time)
                    .ToList();

                foreach (var msg in messages)
                {
                    chat.Messages.Add(new MessageViewModel
                    {
                        Content = msg.Content,
                        Time = msg.Time,
                        IsOwn = msg.SenderId == _managerId
                    });
                }
            }
        }

        public void SendMessage(string text)
        {
            if (SelectedChat == null) return;
            using (var db = new AppDbContext())
            {
                var msg = new Message
                {
                    ChatId = SelectedChat.ChatId,
                    SenderId = _managerId,
                    Content = text,
                    Time = System.DateTime.Now,
                };
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            LoadMessages(SelectedChat.ChatId, SelectedChat.InterlocutorId);
        }
    }
}