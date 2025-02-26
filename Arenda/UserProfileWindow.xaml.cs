using System;
using System.Linq;
using System.Windows;
using Arenda.Data;
using Arenda.Models;

namespace Arenda
{
    public partial class UserProfileWindow : Window
    {
        private readonly AppDbContext _dbContext = new AppDbContext();
        private bool _isEditing = false;
        private User currentUser;

        public UserProfileWindow(User user)
        {
            InitializeComponent();
            _dbContext = new AppDbContext(); // Убедитесь, что создается новый экземпляр
            currentUser = user;
        }

            private void LoadUserData()
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

            if (user != null)
            {
                FullNameTextBox.Text = user.FullName;
                PhoneNumberTextBox.Text = user.PhoneNumber;

                var agreement = _dbContext.ResidentialProperties.FirstOrDefault(r => r.OwnerId == CurrentUser.Id);
            }
            else
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void EditPhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEditing)
            {
                PhoneNumberTextBox.IsReadOnly = false;
                PhoneNumberTextBox.Focus();
                SavePhoneNumberButton.IsEnabled = true;
                _isEditing = true;
            }
        }

        private void SavePhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isEditing)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

                if (user != null)
                {
                    user.PhoneNumber = PhoneNumberTextBox.Text;
                    _dbContext.SaveChanges();
                    MessageBox.Show("Номер телефона сохранен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении номера.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                PhoneNumberTextBox.IsReadOnly = true;
                SavePhoneNumberButton.IsEnabled = false;
                _isEditing = false;
            }
        }

        private void BecomeOwnerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            _dbContext.Dispose();
            base.OnClosed(e);
        }
    }
}
