using System.Windows;
using System.Windows.Controls;
using Arenda.ViewModels;

namespace Arenda
{
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();

            // Пример: создаём фейковую вьюмодель для теста (замени на свою логику)
            var vm = new ChatWindowViewModel();
            DataContext = vm;

            // Пример добавления чатов и сообщений (замени на свою загрузку из БД)
            var chat1 = new ChatViewModel
            {
                DisplayName = "Иван Иванов",
                Avatar = null,
                LastMessage = "Привет!",
                LastMessageDate = "10:30",
                UnreadCount = 2,
                RoleLabel = "Владелец"
            };
            chat1.Messages.Add(new MessageViewModel { Content = "Привет!", Time = System.DateTime.Now, IsOwn = false });
            chat1.Messages.Add(new MessageViewModel { Content = "Добрый день!", Time = System.DateTime.Now, IsOwn = true });
            vm.Chats.Add(chat1);

            var chat2 = new ChatViewModel
            {
                DisplayName = "Петр Петров",
                Avatar = null,
                LastMessage = "Когда встреча?",
                LastMessageDate = "09:15",
                UnreadCount = 0,
                RoleLabel = "Менеджер"
            };
            chat2.Messages.Add(new MessageViewModel { Content = "Когда встреча?", Time = System.DateTime.Now, IsOwn = false });
            vm.Chats.Add(chat2);

            vm.SelectedChat = chat1;
        }

        // Пример отправки сообщения
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChatWindowViewModel vm && vm.SelectedChat != null)
            {
                string text = MessageTextBox.Text;
                if (!string.IsNullOrWhiteSpace(text))
                {
                    vm.SelectedChat.Messages.Add(new MessageViewModel
                    {
                        Content = text,
                        Time = System.DateTime.Now,
                        IsOwn = true
                    });
                    MessageTextBox.Clear();
                }
            }
        }

        // Пример выбора чата из списка
        private void ChatsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ChatWindowViewModel vm)
            {
                vm.SelectedChat = (ChatViewModel)ChatsListBox.SelectedItem;
            }
        }
    }

    // ViewModel для окна чата
    public class ChatWindowViewModel
    {
        public System.Collections.ObjectModel.ObservableCollection<ChatViewModel> Chats { get; set; }
            = new System.Collections.ObjectModel.ObservableCollection<ChatViewModel>();

        private ChatViewModel _selectedChat;
        public ChatViewModel SelectedChat
        {
            get => _selectedChat;
            set => _selectedChat = value;
        }
    }
}