using System;
using System.Collections.ObjectModel;

namespace Arenda.ViewModels
{
    public class ChatViewModel
    {
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public int UnreadCount { get; set; }
        public string RoleLabel { get; set; } // например, "Владелец"
        public ObservableCollection<MessageViewModel> Messages { get; set; } = new();
    }

    public class MessageViewModel
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public bool IsOwn { get; set; }
    }
}