using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace QuanLyDaiLy.Helpers
{
    public class PageNumberToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int pageNumber && parameter is int currentPage)
            {
                // Current page
                if (pageNumber == currentPage)
                    return FontWeights.Medium;

                // Other pages
                return FontWeights.Normal;
            }
            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
