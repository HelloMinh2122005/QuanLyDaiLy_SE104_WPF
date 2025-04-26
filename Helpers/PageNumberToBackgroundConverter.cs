using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QuanLyDaiLy.Helpers
{
    public class PageNumberToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int pageNumber && parameter is int currentPage)
            {
                // Ellipsis or current page
                if (pageNumber == -1)
                    return Brushes.Transparent;

                // Current page
                if (pageNumber == currentPage)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00AAFF"));

                // Other pages
                return Brushes.Transparent;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
