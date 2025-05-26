using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Arenda.Converters
{
    public class MessageBubbleBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOwn)
                return new SolidColorBrush(isOwn ? (Color)ColorConverter.ConvertFromString("#8A7FFF") : (Color)ColorConverter.ConvertFromString("#282840"));
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#282840"));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}