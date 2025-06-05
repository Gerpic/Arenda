using System.Collections.ObjectModel;

namespace Arenda.ViewModels
{
    public class ChatListItemViewModel
    {
        public int ChatId { get; set; }
        public int InterlocutorId { get; set; }
        public string DisplayName { get; set; }
        public string LastMessage { get; set; }
        public string CityAndAddress { get; set; } // Город и адрес одной строкой
        public string UserRole { get; set; } // "Владелец" или "Клиент"
        public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();

        public string TooltipText => $"Объявление: {CityAndAddress}\n{UserRole}";
    }
}