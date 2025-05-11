using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuanLyDaiLy.Models.dto
{
    public class DisplayMatHangPhieuXuat : INotifyPropertyChanged
    {

        public DisplayMatHangPhieuXuat(
            IEnumerable<MatHang> danhSachMatHang)
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
                OnPropertyChanged(nameof(TenDonViTinh));
                OnPropertyChanged(nameof(SoLuongTon));
                OnPropertyChanged(nameof(ThanhTien));
                ThanhTienChanged?.Invoke(this, EventArgs.Empty);
            }
        }

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
