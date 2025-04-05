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
    public class ThemMatHangWindowViewModel : INotifyPropertyChanged
    {
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;

        public ThemMatHangWindowViewModel(IMatHangService matHangService, IDonViTinhService donViTinhService)
        {
            _matHangService = matHangService;
            _donViTinhService = donViTinhService;

            // Initialize commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanMatHangCommand = new RelayCommand(async () => await TiepNhanMatHang());
            MatHangMoiCommand = new RelayCommand(MatHangMoi);

            // Load data
            _ = LoadDonViTinh();
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

        private int _soLuongTon = 0;
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
        public ICommand TiepNhanMatHangCommand { get; }
        public ICommand MatHangMoiCommand { get; }

        // Event to notify parent view when data changes
        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task LoadDonViTinh()
        {
            try
            {
                var donViTinhs = await _donViTinhService.GetAllDonViTinh();
                DonViTinhs = [.. donViTinhs];

                if (DonViTinhs.Count > 0)
                {
                    SelectedDonViTinh = DonViTinhs[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<ThemMatHangWindow>().FirstOrDefault()?.Close();
        }

        private async Task TiepNhanMatHang()
        {
            if (string.IsNullOrWhiteSpace(TenMatHang))
            {
                MessageBox.Show("Tên mặt hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(SelectedDonViTinh.TenDonViTinh))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                MaMatHang = (await _matHangService.GenerateAvailableId()).ToString();

                MatHang matHang = new()
                {
                    MaMatHang = int.Parse(MaMatHang),
                    TenMatHang = TenMatHang,
                    MaDonViTinh = SelectedDonViTinh.MaDonViTinh,
                    SoLuongTon = SoLuongTon
                };

                await _matHangService.AddMatHang(matHang);
                MessageBox.Show("Thêm mặt hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MatHangMoi()
        {
            try
            {
                _ = LoadDonViTinh();
                TenMatHang = string.Empty;
                SoLuongTon = 0;
                MaMatHang = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mặt hàng mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
