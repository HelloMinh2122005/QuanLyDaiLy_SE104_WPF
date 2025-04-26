using System.Globalization;
using System.Windows.Data;

namespace QuanLyDaiLy.Helpers
{
    public class PageNumberToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int pageNumber)
            {
                // -1 is used to represent ellipsis
                return pageNumber == -1 ? "..." : pageNumber.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}