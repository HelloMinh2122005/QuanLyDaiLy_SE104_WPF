using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.DonViTinhViews;

namespace QuanLyDaiLy.ViewModels.DonViTinhViewModels
{
    public partial class ThemDonViTinhPageViewModel : ObservableObject
    {
        private readonly IDonViTinhService _donViTinhService;

        public ThemDonViTinhPageViewModel(IDonViTinhService donViTinhService)
        {
            _donViTinhService = donViTinhService;
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maDonViTinh = "";

        [ObservableProperty]
        private string _tenDonViTinh = "";

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ThemDonViTinhWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TiepNhanDonViTinh()
        {
            if (string.IsNullOrWhiteSpace(TenDonViTinh))
            {
                MessageBox.Show("Tên đơn vị tính không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                MaDonViTinh = (await _donViTinhService.GenerateAvailableId()).ToString();

                DonViTinh donViTinh = new()
                {
                    MaDonViTinh = int.Parse(MaDonViTinh),
                    TenDonViTinh = TenDonViTinh,
                };

                await _donViTinhService.AddDonViTinh(donViTinh);
                MessageBox.Show("Thêm đơn vị tính thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void DonViTinhMoi()
        {
            try
            {
                MaDonViTinh = string.Empty;
                TenDonViTinh = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo đơn vị tính mới: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
