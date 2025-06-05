using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Arenda.Data;
using Arenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Arenda.Windows
{
    public partial class AdminWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly int _adminId;

        public AdminWindow(int adminId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _adminId = adminId;
            LoadProperties();
        }

        public AdminWindow() : this(1) // Если нужен дефолтный id
        {
        }

        private void LoadProperties()
        {
            var properties = _dbContext.ResidentialProperties
                .Include(p => p.Owner)
                .Include(p => p.City)
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .Select(p => new PropertyListItem
                {
                    Id = p.Id,
                    OwnerName = p.Owner.FullName,
                    City = p.City.Name,
                    Address = p.Address,
                    Area = p.Area,
                    Price = p.Price,
                    Category = p.Category.CategoryName
                }).ToList();

            PropertiesListView.ItemsSource = properties;
        }

        private void PropertiesListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PropertiesListView.SelectedItem is PropertyListItem item)
            {
                var detailsWindow = new EditPropertyWindow(item.Id);
                detailsWindow.Show();
                this.Close();
            }
        }

        private void SupportChat_Click(object sender, RoutedEventArgs e)
        {
            var chatWindow = new SupportChatWindow(_adminId);
            chatWindow.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LogInWindow();
            loginWindow.Show();
            this.Close();
        }
    }

    public class PropertyListItem
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public double Area { get; set; }
        public decimal Price { get; set; }
    }
}