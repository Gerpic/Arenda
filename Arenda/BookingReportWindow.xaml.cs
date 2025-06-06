using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Arenda.Models;
using Arenda.Data;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace Arenda
{
    public partial class BookingReportWindow : Window
    {
        private readonly int _userId;
        private readonly AppDbContext _dbContext;

        public BookingReportWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _dbContext = new AppDbContext();
        }

        private async void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime from = FromDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime to = ToDatePicker.SelectedDate ?? DateTime.MaxValue;

            var bookings = await _dbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Property)
                    .ThenInclude(p => p.Owner)
                .Where(b => b.StartDate >= from && b.EndDate <= to)
                .ToListAsync();

            var report = bookings.Select(b => new BookingReportEntry
            {
                BookingId = b.Id,
                PropertyName = b.Property?.Address ?? "—",
                ClientName = b.User?.FullName ?? "—",
                ManagerName = b.Property?.Owner?.FullName ?? "—",
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Status = b.Status,
                Price = b.Property?.Price ?? 0
            }).ToList();

            ReportListView.ItemsSource = report;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные из ListView
            var items = ReportListView.ItemsSource as IEnumerable<BookingReportEntry>;
            if (items == null || !items.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Экспорт", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = "report.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("ID;Адрес;Клиент;Менеджер;Дата начала;Дата окончания;Статус;Сумма");
                    foreach (var item in items)
                    {
                        // Экранирование ; и переносов строк, если вдруг они есть в текстах
                        sb.AppendLine(
                            $"{EscapeCsv(item.BookingId.ToString())};" +
                            $"{EscapeCsv(item.PropertyName)};" +
                            $"{EscapeCsv(item.ClientName)};" +
                            $"{EscapeCsv(item.ManagerName)};" +
                            $"{item.StartDate:dd.MM.yyyy};" +
                            $"{item.EndDate:dd.MM.yyyy};" +
                            $"{EscapeCsv(item.Status)};" +
                            $"{item.Price:n2}"
                        );
                    }

                    File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Экспорт завершён!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Помощник для экранирования значений (на всякий случай)
        private string EscapeCsv(string value)
        {
            if (value == null) return "";
            if (value.Contains(";") || value.Contains("\n") || value.Contains("\""))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var managerWindow = new ManagerWindow(_userId);
            managerWindow.Show();
            this.Close();
        }
    }

    public class BookingReportEntry
    {
        public int BookingId { get; set; }
        public string PropertyName { get; set; }
        public string ClientName { get; set; }
        public string ManagerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}