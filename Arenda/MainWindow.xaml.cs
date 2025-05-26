using System;
using System.Linq;
using System.Windows;
using Arenda.Data;
using Arenda.Models;

namespace Arenda
{
    public partial class LogIn : Window
    {
        private readonly AppDbContext _dbContext = new AppDbContext();
        private bool isPasswordVisible = false;

        public LogIn()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == LoginTextBox.Text);

                if (user != null && user.Password == PasswordBox.Password)
                {
                    CurrentUser.Id = user.Id;
                    CurrentUser.RoleId = user.RoleId; // сохраняем id роли

                    // Открываем нужное окно в зависимости от роли
                    if (user.RoleId == 1)
                    {
                        var adminWindow = new AdminWindow(CurrentUser.Id);
                        adminWindow.Show();
                    }
                    else if (user.RoleId == 2)
                    {
                        var managerWindow = new ManagerWindow(CurrentUser.Id);
                        managerWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            PasswordTextBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = isPasswordVisible ? Visibility.Collapsed : Visibility.Visible;
            PasswordTextBox.Visibility = isPasswordVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public static class CurrentUser
    {
        public static int Id { get; set; }
        public static int RoleId { get; set; }
    }
}