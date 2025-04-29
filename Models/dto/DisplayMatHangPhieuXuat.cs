using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace QuanLyDaiLy.Models.dto
{
    public class DisplayMatHangPhieuXuat : INotifyPropertyChanged
    {
        //private readonly ObservableCollection<DisplayMatHangPhieuXuat> _parentList;

        public DisplayMatHangPhieuXuat(
            IEnumerable<MatHang> danhSachMatHang)
            //ObservableCollection<DisplayMatHangPhieuXuat> parentList)
        {
            DanhSachMatHang = [.. danhSachMatHang];
            if (DanhSachMatHang.Count > 0)
                SelectedMatHang = DanhSachMatHang[0];
            //_parentList = parentList;
        }


        private ObservableCollection<MatHang> _danhSachMatHang = [];
        public ObservableCollection<MatHang> DanhSachMatHang
        {
            get => _danhSachMatHang;
            set
            {
                _danhSachMatHang = value;
                OnPropertyChanged();
            }
        }

        private MatHang _selectedMatHang = null!;
        public MatHang SelectedMatHang
        {
            get => _selectedMatHang;
            set
            {
                _selectedMatHang = value;
                OnPropertyChanged();
                // Also update these dependent properties when SelectedMatHang changes
                OnPropertyChanged(nameof(TenDonViTinh));
                OnPropertyChanged(nameof(SoLuongTon));
                OnPropertyChanged(nameof(ThanhTien));
                ThanhTienChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        //private MatHang _selectedMatHang = null!;
        //public MatHang SelectedMatHang
        //{
        //    get => _selectedMatHang;
        //    set
        //    {
        //        if (value == _selectedMatHang)
        //            return;

        //        // Kiểm tra trùng lặp trong parent list
        //        bool isDup = _parentList != null &&
        //                     _parentList.Where(x => x != this)
        //                                .Any(x => x.SelectedMatHang.MaMatHang == value.MaMatHang);
        //        if (isDup)
        //        {
        //            MessageBox.Show(
        //                $"Không được phép chọn mặt hàng \"{value.TenMatHang}\" vì đã tồn tại! \n Vui lòng chọn lại!",
        //                "Cảnh báo",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Warning);
        //            // Bỏ qua gán để UI revert về giá trị cũ
        //            OnPropertyChanged(nameof(SelectedMatHang));
        //            return;
        //        }
        //        else
        //        {
        //            _selectedMatHang = value;
        //            OnPropertyChanged();
        //        }    
        //        // Gán giá trị mới                
        //        OnPropertyChanged(nameof(TenDonViTinh));
        //        OnPropertyChanged(nameof(SoLuongTon));
        //        OnPropertyChanged(nameof(ThanhTien));
        //        ThanhTienChanged?.Invoke(this, EventArgs.Empty);
        //    }
        //}

        // Add these properties to expose the DonViTinh and SoLuongTon from SelectedMatHang
        public string TenDonViTinh => SelectedMatHang?.DonViTinh?.TenDonViTinh ?? string.Empty;

        public int SoLuongTon => SelectedMatHang?.SoLuongTon ?? 0;

        private int _soLuongXuat = 0;
        public int SoLuongXuat
        {
            get => _soLuongXuat;
            set
            {
                _soLuongXuat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ThanhTien));
                ThanhTienChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private long _donGiaXuat = 0;
        public long DonGiaXuat
        {
            get => _donGiaXuat;
            set
            {
                _donGiaXuat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ThanhTien));
                ThanhTienChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public long ThanhTien => SoLuongXuat * DonGiaXuat;

        public event PropertyChangedEventHandler? PropertyChanged;
        // Add an event to notify when ThanhTien changes
        public event EventHandler? ThanhTienChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
