using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QuanLyDaiLy.Helpers
{
    public class PageNumberToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int pageNumber && parameter is int currentPage)
            {
                // Current page
                if (pageNumber == currentPage)
                    return Brushes.White;

                // Other pages or ellipsis
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
