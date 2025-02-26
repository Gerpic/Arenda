using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Arenda
{
    public partial class Registration : Window
    {
        private string connectionString = "Server=localhost;Port=5432;Username=postgres;Password=12345;Database=Arenda";

        private bool isPasswordVisible = false;
        public Registration()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Назад"
        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новое окно авторизации
            LogIn loginWindow = new LogIn();

            // Отображаем окно авторизации
            loginWindow.Show();

            // Закрываем текущее окно регистрации
            this.Close();
        }


        // Обработчик для кнопки "Регистрация"
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!EmailTextBox.Text.Contains("@"))
            {
                MessageBox.Show("Введите корректный адрес электронной почты!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO users (username, email, password, phone_number, role_id, is_owner) " +
                                   "VALUES (@username, @email, @password, @phone_number, @role_id, @is_owner)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", FullNameTextBox.Text);
                        command.Parameters.AddWithValue("@email", EmailTextBox.Text);
                        command.Parameters.AddWithValue("@password", PasswordBox.Password); // Здесь нужно хешировать пароль
                        command.Parameters.AddWithValue("@phone_number", PhoneNumberTextBox.Text);
                        command.Parameters.AddWithValue("@role_id", 2); // По умолчанию "Пользователь"
                        command.Parameters.AddWithValue("@is_owner", false);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                LogIn loginWindow = new LogIn();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик маски ввода номера телефона
        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"\d"); // Только цифры
        }

        // Маска для номера телефона
        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string input = textBox.Text;

            // Убираем все символы, кроме цифр
            string numbers = Regex.Replace(input, @"[^\d]", "");

            // Добавляем +7 и форматируем остальную часть
            if (numbers.StartsWith("7"))
            {
                numbers = numbers.Substring(1); // Убираем лишнюю 7, если пользователь ввел ее вручную
            }

            string formatted = "+7(";
            if (numbers.Length > 0) formatted += numbers.Substring(0, Math.Min(3, numbers.Length));
            if (numbers.Length > 3) formatted += ")" + numbers.Substring(3, Math.Min(3, numbers.Length - 3));
            if (numbers.Length > 6) formatted += "-" + numbers.Substring(6, Math.Min(2, numbers.Length - 6));
            if (numbers.Length > 8) formatted += "-" + numbers.Substring(8, Math.Min(2, numbers.Length - 8));

            textBox.Text = formatted;

            // Устанавливаем курсор в конец текста
            textBox.CaretIndex = textBox.Text.Length;
        }


        // Обработчик показа/скрытия пароля
        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
        }
        private void ToggleConfirmPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                ConfirmPasswordTextBox.Text = ConfirmPasswordBox.Password;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                ConfirmPasswordBox.Password = ConfirmPasswordTextBox.Text;
                ConfirmPasswordTextBox.Visibility = Visibility.Collapsed;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
            }
        }

    }
}
