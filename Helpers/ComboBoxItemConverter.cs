using System.Globalization;
using System.Windows.Data;
using QuanLyDaiLy.Models;

namespace QuanLyDaiLy.Helpers
{
    public class ComboBoxItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (value is LoaiDaiLy loaiDaiLy)
                return loaiDaiLy.TenLoaiDaiLy;

            if (value is Quan quan)
                return quan.TenQuan;

            if (value is DaiLy daiLy)
                return daiLy.TenDaiLy;

            if (value is MatHang matHang)
                return matHang.TenMatHang;

            if (value is DonViTinh donViTinh)
                return donViTinh.TenDonViTinh;

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This converter is only used for display, so we don't need to implement ConvertBack
            throw new NotImplementedException();
        }
    }
}