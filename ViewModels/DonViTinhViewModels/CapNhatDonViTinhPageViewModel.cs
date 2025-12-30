using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.DonViTinhViews;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.DonViTinhViewModels
{
    public partial class CapNhatDonViTinhPageViewModel :
        ObservableObject,
        IRecipient<SelectedIdMessage>
    {
        private readonly IDonViTinhService _donViTinhService;
        private int _donViTinhId;

        public CapNhatDonViTinhPageViewModel(IDonViTinhService donViTinhService)
        {
            _donViTinhService = donViTinhService;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(SelectedIdMessage message)
        {
            _donViTinhId = message.Value;
            // Load data
            _ = LoadDataAsync();
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maDonViTinh = "";

        [ObservableProperty]
        private string _tenDonViTinh = "";

        // Methods
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

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<CapNhatDonViTinhWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task EditDonViTinh()
        {
            if (string.IsNullOrWhiteSpace(TenDonViTinh))
            {
                MessageBox.Show("Tên đơn vị tính không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingDonViTinh = await _donViTinhService.GetDonViTinhById(_donViTinhId);

                existingDonViTinh.TenDonViTinh = TenDonViTinh;

                await _donViTinhService.UpdateDonViTinh(existingDonViTinh);

                MessageBox.Show("Cập nhật đơn vị tính thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
