using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Arenda.Data;
using Arenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Arenda.Windows
{
    public partial class ListingRequestsWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly int _managerId;

        public ListingRequestsWindow(int managerId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _managerId = managerId;
            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = _dbContext.PropertyListingRequests
                .Include(r => r.SubmittedByUser)
                .Include(r => r.City)
                .OrderByDescending(r => r.SubmissionDate)
                .ToList()
                .Select(r => new ListingRequestListItem
                {
                    Id = r.Id,
                    OwnerName = r.SubmittedByUser.FullName,
                    City = r.City.Name,
                    Address = r.Address,
                    Status = r.Status,
                    SubmissionDate = r.SubmissionDate
                })
                .ToList();

            RequestsListView.ItemsSource = requests;
        }

        private void RequestsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestsListView.SelectedItem is ListingRequestListItem item)
            {
                var detailsWindow = new ListingRequestDetailsWindow(item.Id, _managerId);
                detailsWindow.Show();
                this.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var managerWindow = new ManagerWindow(_managerId);
            managerWindow.Show();
            this.Close();
        }
    }

    public class ListingRequestListItem
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public System.DateTime SubmissionDate { get; set; }
    }
}