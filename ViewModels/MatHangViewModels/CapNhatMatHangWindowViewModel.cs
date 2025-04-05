using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public class CapNhatMatHangWindowViewModel : INotifyPropertyChanged
    {
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;
        private readonly int _matHangId;

        public CapNhatMatHangWindowViewModel(
            IMatHangService matHangService,
            IDonViTinhService donViTinhService,
            int matHangId)
        {
            _matHangService = matHangService;
            _matHangId = matHangId;
            _donViTinhService = donViTinhService;

            // Initialize commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            CapNhatMatHangCommand = new RelayCommand(async () => await CapNhatMatHang());

            // Load data
            _ = LoadDataAsync();
        }

        // Binding properties
        private string _maMatHang = "";
        public string MaMatHang
        {
            get => _maMatHang;
            set
            {
                _maMatHang = value;
                OnPropertyChanged();
            }
        }

        private string _tenMatHang = "";
        public string TenMatHang
        {
            get => _tenMatHang;
            set
            {
                _tenMatHang = value;
                OnPropertyChanged();
            }
        }

        private int _soLuongTon;
        public int SoLuongTon
        {
            get => _soLuongTon;
            set
            {
                _soLuongTon = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DonViTinh> _donViTinhs = [];
        public ObservableCollection<DonViTinh> DonViTinhs
        {
            get => _donViTinhs;
            set
            {
                _donViTinhs = value;
                OnPropertyChanged();
            }
        }

        private DonViTinh _selectedDonViTinh = null!;
        public DonViTinh SelectedDonViTinh
        {
            get => _selectedDonViTinh;
            set
            {
                _selectedDonViTinh = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand CapNhatMatHangCommand { get; }

        // Events
        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        // Methods
        private async Task LoadDataAsync()
        {
            try
            {
                var donViTinhs = await _donViTinhService.GetAllDonViTinh();
                DonViTinhs = [.. donViTinhs];

                var matHang = await _matHangService.GetMatHangById(_matHangId);
                MaMatHang = matHang.MaMatHang.ToString();
                TenMatHang = matHang.TenMatHang;
                SoLuongTon = matHang.SoLuongTon;

                SelectedDonViTinh = DonViTinhs.FirstOrDefault(d => d.MaDonViTinh == matHang.MaDonViTinh) ?? DonViTinhs.FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<CapNhatMatHangWindow>().FirstOrDefault()?.Close();
        }

        private async Task CapNhatMatHang()
        {
            if (string.IsNullOrWhiteSpace(TenMatHang))
            {
                MessageBox.Show("Tên mặt hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Get existing mat hang
                var existingMatHang = await _matHangService.GetMatHangById(_matHangId);

                // Update properties
                existingMatHang.TenMatHang = TenMatHang;
                existingMatHang.MaDonViTinh = SelectedDonViTinh.MaDonViTinh;
                existingMatHang.SoLuongTon = SoLuongTon;

                // Save changes
                await _matHangService.UpdateMatHang(existingMatHang);

                MessageBox.Show("Cập nhật mặt hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
