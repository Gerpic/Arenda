using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Arenda.Data;
using Arenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Arenda.Windows
{
    public partial class EditPropertyWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly int _propertyId;
        private ResidentialProperty _property;
        private List<Review> _reviews;

        public EditPropertyWindow(int propertyId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _propertyId = propertyId;
            LoadData();
        }

        private void LoadData()
        {
            CityBox.ItemsSource = _dbContext.Cities.ToList();
            CategoryBox.ItemsSource = _dbContext.PropertyCategories.ToList();

            _property = _dbContext.ResidentialProperties
                .Include(p => p.Owner)
                .Include(p => p.City)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == _propertyId);

            if (_property == null)
            {
                MessageBox.Show("Объявление не найдено.");
                Close();
                return;
            }

            OwnerBox.Text = _property.Owner?.FullName ?? "";
            CityBox.SelectedItem = _property.City;
            CategoryBox.SelectedItem = _property.Category;
            AddressBox.Text = _property.Address;
            AreaBox.Text = _property.Area.ToString("F2");
            PriceBox.Text = _property.Price.ToString("F2");
            DescriptionBox.Text = _property.Description;
            RoomCountBox.Text = _property.RoomCount.ToString();
            CapacityBox.Text = _property.Capacity.ToString();

            // Загрузка отзывов для этого объекта
            _reviews = _dbContext.Reviews
                .Include(r => r.User)
                .Where(r => r.PropertyId == _propertyId)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();

            ReviewsList.ItemsSource = _reviews;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_property == null) return;

            _property.CityId = ((City)CityBox.SelectedItem)?.Id ?? _property.CityId;
            _property.CategoryId = ((PropertyCategory)CategoryBox.SelectedItem)?.Id ?? _property.CategoryId;
            _property.Address = AddressBox.Text;
            if (double.TryParse(AreaBox.Text, out double area))
                _property.Area = area;
            if (decimal.TryParse(PriceBox.Text, out decimal price))
                _property.Price = price;
            _property.Description = DescriptionBox.Text;
            if (int.TryParse(RoomCountBox.Text, out int rooms))
                _property.RoomCount = rooms;
            if (int.TryParse(CapacityBox.Text, out int cap))
                _property.Capacity = cap;

            _dbContext.SaveChanges();
            MessageBox.Show("Изменения сохранены.");
            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_property == null) return;
            var result = MessageBox.Show("Вы уверены, что хотите удалить объявление?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _dbContext.ResidentialProperties.Remove(_property);
                _dbContext.SaveChanges();
                MessageBox.Show("Объявление удалено.");
                Close();
            }
        }

        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int reviewId)
            {
                var result = MessageBox.Show("Удалить этот отзыв?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var review = _dbContext.Reviews.Find(reviewId);
                    if (review != null)
                    {
                        _dbContext.Reviews.Remove(review);
                        _dbContext.SaveChanges();
                        // Обновляем список отзывов
                        _reviews.RemoveAll(r => r.Id == reviewId);
                        ReviewsList.ItemsSource = null;
                        ReviewsList.ItemsSource = _reviews;
                    }
                }
            }
        }

        // Кнопка назад — закрывает текущее окно и открывает окно администратора
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
    }
}