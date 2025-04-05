using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Commands;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.DonViTinhViews;

namespace QuanLyDaiLy.ViewModels.DonViTinhViewModels
{
    public class CapNhatDonViTinhPageViewModel : INotifyPropertyChanged
    {
        private readonly IDonViTinhService _donViTinhService;
        private readonly int _donViTinhId;

        public CapNhatDonViTinhPageViewModel(
            IDonViTinhService donViTinhService,
            int donViTinhId
        )
        {
            _donViTinhId = donViTinhId;
            _donViTinhService = donViTinhService;

            CloseWindowCommand = new RelayCommand(CloseWindow);
            EditDonViTinhCommand = new RelayCommand(async () => await CapNhatDonViTinh());

            _ = LoadDataAsync();
        }

        // Properties for binding
        private string _maDonViTinh = string.Empty;
        public string MaDonViTinh
        {
            get => _maDonViTinh;
            set
            {
                _maDonViTinh = value;
                OnPropertyChanged();
            }
        }

        private string _tenDonViTinh = string.Empty;
        public string TenDonViTinh
        {
            get => _tenDonViTinh;
            set
            {
                _tenDonViTinh = value;
                OnPropertyChanged();
            }
        }

        // Event for notifying parent components
        public event EventHandler? DataChanged;

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand EditDonViTinhCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                var donViTinh = await _donViTinhService.GetDonViTinhById(_donViTinhId);
                MaDonViTinh = donViTinh.MaDonViTinh.ToString();
                TenDonViTinh = donViTinh.TenDonViTinh;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<CapNhatDonViTinhWindow>().FirstOrDefault()?.Close();
        }

        private async Task CapNhatDonViTinh()
        {
            if (string.IsNullOrWhiteSpace(TenDonViTinh))
            {
                MessageBox.Show("Tên đơn vị tính không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingDonViTinh = await _donViTinhService.GetDonViTinhById(_donViTinhId);

                existingDonViTinh.TenDonViTinh = TenDonViTinh;

                await _donViTinhService.UpdateDonViTinh(existingDonViTinh);
                MessageBox.Show("Cập nhật đơn vị tính thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
