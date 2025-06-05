using System;
using System.Collections.ObjectModel;

namespace Arenda.ViewModels
{
    public class ChatViewModel
    {
        public int ChatId { get; set; }
        public string FullName { get; set; }
        public string PropertyAddress { get; set; }
        public bool IsOwner { get; set; }
        public string DisplayName => $"{FullName}{(IsOwner ? " (владелец)" : "")}";
        public string LastMessage { get; set; }
        public string LastMessageDate { get; set; }
        public int UnreadCount { get; set; }
        public int InterlocutorId { get; set; }
        public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();
    }
}