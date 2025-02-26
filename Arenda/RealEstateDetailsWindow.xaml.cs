using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Arenda.Data;
using Arenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Arenda.Windows
{
    public partial class RealEstateDetailsWindow : Window
    {
        public RealEstateDetailsWindow(int propertyId)
        {
            InitializeComponent();
            LoadPropertyDetails(propertyId);
        }

        private void LoadPropertyDetails(int propertyId)
        {
            using (var context = new AppDbContext())
            {
                var property = context.ResidentialProperties
                    .Include(p => p.Category)
                    .Include(p => p.Owner)
                    .Include(p => p.Photos)
                    .FirstOrDefault(p => p.Id == propertyId);

                if (property == null)
                {
                    MessageBox.Show("Недвижимость не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    // Вместо Close() скрываем окно
                    this.Visibility = Visibility.Hidden;
                    return;
                }


                // Заполнение UI данными
                CategoryTextBlock.Text = $"Категория: {property.Category?.CategoryName}";
                AddressTextBlock.Text = $"Адрес: {property.Address}";
                AreaTextBlock.Text = $"Площадь: {property.Area} м²";
                PriceTextBlock.Text = $"Цена: {property.Price} руб.";
                DescriptionTextBlock.Text = $"Описание: {property.Description}";

                if (property.Owner != null)
                {
                    OwnerInfoTextBlock.Text = $"Владелец: {property.Owner.FullName}";
                    OwnerPhoneTextBlock.Text = $"Контактный номер: {property.Owner.PhoneNumber}";
                }
                else
                {
                    OwnerInfoTextBlock.Text = "Владелец: Неизвестно";
                    OwnerPhoneTextBlock.Text = "Контактный номер: Неизвестно";
                }

                // Загружаем фотографии
                PhotosList.Items.Clear();
                foreach (var photoUrl in property.Photos.Select(photo => photo.PhotoUrl))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(photoUrl, UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();

                    PhotosList.Items.Add(new Image { Source = bitmap, Width = 300, Height = 200, Margin = new Thickness(10) });
                }
            }
        }

    }
}
