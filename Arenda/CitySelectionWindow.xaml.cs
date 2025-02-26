using System.Windows;
using Npgsql;
using System.Collections.Generic;
using System;

namespace Arenda
{
    public partial class CitySelectionWindow : Window
    {
        public string SelectedCity { get; private set; }
        private readonly string connectionString = "Server=localhost;Port=5432;Username=postgres;Password=12345;Database=Arenda";

        public CitySelectionWindow()
        {
            InitializeComponent();
            LoadCitiesFromDatabase();
        }

        private void LoadCitiesFromDatabase()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT name FROM cities";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var cities = new List<string>();
                        while (reader.Read())
                        {
                            cities.Add(reader.GetString(0));
                        }
                        CityListBox.ItemsSource = cities;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки городов: {ex.Message}");
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (CityListBox.SelectedItem != null)
            {
                SelectedCity = CityListBox.SelectedItem.ToString();
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите город!");
            }
        }
    }
}