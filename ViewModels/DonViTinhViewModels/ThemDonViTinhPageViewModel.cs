using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace QuanLyDaiLy.ViewModels.DonViTinhViewModels
{
    public class ThemDonViTinhPageViewModel : INotifyPropertyChanged
    {
        private readonly IDonViTinhService _donViTinhService;

        public ThemDonViTinhPageViewModel(IDonViTinhService donViTinhService)
        {
            _donViTinhService = donViTinhService;

            // Initialize commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanDonViTinhCommand = new RelayCommand(async () => await TiepNhanDonViTinh());
            DonViTinhMoiCommand = new RelayCommand(DonViTinhMoi);
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

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand TiepNhanDonViTinhCommand { get; }
        public ICommand DonViTinhMoiCommand { get; }

        // Events
        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<Views.DonViTinhViews.ThemDonViTinhWindow>().FirstOrDefault()?.Close();
        }

        private async Task TiepNhanDonViTinh()
        {
            if (string.IsNullOrWhiteSpace(TenDonViTinh))
            {
                MessageBox.Show("Tên đơn vị tính không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MaDonViTinh = (await _donViTinhService.GenerateAvailableId()).ToString();

            DonViTinh donViTinh = new()
            {
                MaDonViTinh = int.Parse(MaDonViTinh),
                TenDonViTinh = TenDonViTinh
            };

            try
            {
                await _donViTinhService.AddDonViTinh(donViTinh);
                MessageBox.Show("Tiếp nhận đơn vị tính thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lưu đơn vị tính không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DonViTinhMoi()
        {
            TenDonViTinh = string.Empty;
            MaDonViTinh = string.Empty;
        }
    }
}
