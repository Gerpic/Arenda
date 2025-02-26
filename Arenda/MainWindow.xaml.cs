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

                    // Открываем окно выбора города
                    var cityWindow = new CitySelectionWindow();
                    if (cityWindow.ShowDialog() == true && !string.IsNullOrEmpty(cityWindow.SelectedCity))
                    {
                        // Исправлено: передаем оба параметра
                        var mainMenu = new MainMenu(CurrentUser.Id, cityWindow.SelectedCity);
                        mainMenu.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Выберите город для продолжения!");
                    }
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();
            registrationWindow.Show();
            this.Close();
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
    }
}