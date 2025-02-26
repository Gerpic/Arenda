using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Arenda.Data;
using Arenda.Models;
using Arenda.Windows;
using Microsoft.EntityFrameworkCore;

namespace Arenda
{
    public partial class MainMenu : Window
    {
        private string selectedCity;
        private int loggedInUserId;
        private readonly AppDbContext _dbContext;

        // Исправленный конструктор с двумя параметрами
        public MainMenu(int userId, string city)
        {
            InitializeComponent();
            loggedInUserId = userId;
            selectedCity = city;
            _dbContext = new AppDbContext();

            Loaded += async (s, e) =>
            {
                await LoadCategoriesAsync();
                await LoadRealEstatesAsync();
            };
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _dbContext.PropertyCategories
                    .Select(c => c.CategoryName)
                    .ToListAsync();

                CategoriesFilterPanel.Children.Clear();

                foreach (var category in categories)
                {
                    var checkBox = new CheckBox
                    {
                        Content = category,
                        Margin = new Thickness(0, 5, 0, 0)
                    };
                    CategoriesFilterPanel.Children.Add(checkBox);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }

        private async Task LoadRealEstatesAsync(
            List<string> categories = null,
            decimal? minArea = null,
            decimal? maxArea = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? roomCount = null)
        {
            try
            {
                RealEstateListView.ItemsSource = null;

                var query = _dbContext.ResidentialProperties
                    .Include(r => r.Category)
                    .Include(r => r.City)
                    .Include(r => r.Photos)
                    .Where(r => r.City.Name == selectedCity);

                if (categories?.Any() == true)
                    query = query.Where(r => categories.Contains(r.Category.CategoryName));

                if (minArea.HasValue)
                    query = query.Where(r => r.Area >= minArea);

                if (maxArea.HasValue)
                    query = query.Where(r => r.Area <= maxArea);

                if (minPrice.HasValue)
                    query = query.Where(r => r.Price >= minPrice);

                if (maxPrice.HasValue)
                    query = query.Where(r => r.Price <= maxPrice);

                if (roomCount.HasValue)
                    query = roomCount == 5
                        ? query.Where(r => r.RoomCount >= 5)
                        : query.Where(r => r.RoomCount == roomCount);

                var result = await query
                    .Select(r => new RealEstate
                    {
                        Id = r.Id,
                        Address = r.Address,
                        Category = r.Category.CategoryName,
                        Area = r.Area,
                        Price = r.Price,
                        Rooms = r.RoomCount,
                        Photos = r.Photos.Select(p => p.PhotoUrl).ToArray()
                    })
                    .ToListAsync();

                RealEstateListView.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки недвижимости: {ex.Message}");
            }
        }

        private void RealEstateListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RealEstateListView.SelectedItem is RealEstate estate)
            {
                new RealEstateDetailsWindow(estate.Id).ShowDialog();
                RealEstateListView.SelectedItem = null;
            }
        }

        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == loggedInUserId);

                if (user != null)
                {
                    new UserProfileWindow(user).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка профиля: {ex.Message}");
            }
        }
    }
}