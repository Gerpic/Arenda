using System;
using System.Linq;
using System.Windows;
using Arenda.Data;
using Arenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Arenda.Windows
{
    public partial class ListingRequestDetailsWindow : Window
    {
        private readonly AppDbContext _dbContext;
        private readonly int _requestId;
        private readonly int _managerId;
        private PropertyListingRequest _request;

        public ListingRequestDetailsWindow(int requestId, int managerId)
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _requestId = requestId;
            _managerId = managerId;
            LoadDetails();
        }

        private void LoadDetails()
        {
            _request = _dbContext.PropertyListingRequests
                .Include(r => r.SubmittedByUser)
                .Include(r => r.Category)
                .Include(r => r.City)
                .Include(r => r.Photos)
                .FirstOrDefault(r => r.Id == _requestId);

            if (_request == null)
            {
                MessageBox.Show("Заявление не найдено.");
                Close();
                return;
            }

            OwnerText.Text = _request.SubmittedByUser.FullName;
            CategoryText.Text = _request.Category.CategoryName;
            CityText.Text = _request.City.Name;
            AddressText.Text = _request.Address;
            AreaText.Text = _request.Area.ToString("F2");
            DescriptionText.Text = _request.Description;
            RoomCountText.Text = _request.RoomCount.ToString();
            CapacityText.Text = _request.Capacity.ToString();
            PriceText.Text = _request.Price.ToString("F2");
            StatusText.Text = _request.Status;
            SubmissionDateText.Text = _request.SubmissionDate.ToString("dd.MM.yyyy HH:mm");
            RejectionReasonPanel.Visibility = _request.Status == "отклонено" ? Visibility.Visible : Visibility.Collapsed;
            RejectionReasonText.Text = _request.RejectionReason ?? "";

            ApproveButton.IsEnabled = _request.Status == "ожидает рассмотрения";
            RejectButton.IsEnabled = _request.Status == "ожидает рассмотрения";
            RejectReasonPanel.Visibility = Visibility.Collapsed;
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_request == null) return;

            var newProperty = new ResidentialProperty
            {
                Address = _request.Address,
                Area = Convert.ToDouble(_request.Area),
                Description = _request.Description,
                OwnerId = _request.SubmittedBy,
                CategoryId = _request.CategoryId,
                CityId = _request.CityId,
                RoomCount = _request.RoomCount,
                Capacity = _request.Capacity,
                Price = Convert.ToDecimal(_request.Price)
            };
            _dbContext.ResidentialProperties.Add(newProperty);
            _dbContext.SaveChanges();

            foreach (var reqPhoto in _request.Photos)
            {
                _dbContext.PropertyPhotos.Add(new PropertyPhoto
                {
                    PropertyId = newProperty.Id,
                    PhotoUrl = reqPhoto.PhotoUrl
                });
            }

            _request.Status = "одобрено";
            _request.ProcessedBy = _managerId;
            _request.ProcessingDate = DateTime.Now;
            _request.RejectionReason = null;

            _dbContext.SaveChanges();

            MessageBox.Show("Заявление одобрено и объявление создано.");
            Close();
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            RejectReasonPanel.Visibility = Visibility.Visible;
        }

        private void ConfirmRejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_request == null) return;
            string reason = RejectReasonBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Укажите причину отклонения.");
                return;
            }
            _request.Status = "отклонено";
            _request.ProcessedBy = _managerId;
            _request.ProcessingDate = DateTime.Now;
            _request.RejectionReason = reason;
            _dbContext.SaveChanges();
            MessageBox.Show("Заявление отклонено.");
            Close();
        }

        // Кнопка назад — закрывает текущее окно и открывает окно заявлений
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var listingRequestsWindow = new ListingRequestsWindow(_managerId);
            listingRequestsWindow.Show();
            this.Close();
        }
    }
}