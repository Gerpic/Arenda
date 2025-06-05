using System;
using System.Windows;
using System.Windows.Controls;
using Arenda.ViewModels;

namespace Arenda.Windows
{
    public partial class ChatWindow : Window
    {
        private ChatWindowViewModel vm;
        private int _managerId;

        public ChatWindow(int managerId)
        {
            InitializeComponent();
            _managerId = managerId;
            vm = new ChatWindowViewModel(managerId);
            DataContext = vm;
        }

        public ChatWindow(int managerId, int chatId, int interlocutorId)
        {
            InitializeComponent();
            _managerId = managerId;
            vm = new ChatWindowViewModel(managerId, chatId, interlocutorId);
            DataContext = vm;
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm?.SelectedChat != null)
            {
                string text = MessageTextBox.Text;
                if (!string.IsNullOrWhiteSpace(text))
                {
                    vm.SendMessage(text);
                    MessageTextBox.Clear();
                    ScrollMessagesToEnd();
                }
            }
        }

        private void ChatsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChatsListBox.SelectedItem is ChatListItemViewModel chat && chat != vm.SelectedChat)
            {
                vm.SelectedChat = chat;
                vm.LoadMessages(chat.ChatId, chat.InterlocutorId);
                MessagesPanel.ItemsSource = null;
                MessagesPanel.ItemsSource = chat.Messages;
                ScrollMessagesToEnd();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var managerWindow = new ManagerWindow(_managerId);
            managerWindow.Show();
            this.Close();
        }

        private void ScrollMessagesToEnd()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                MessagesScrollViewer?.ScrollToEnd();
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
    }
}