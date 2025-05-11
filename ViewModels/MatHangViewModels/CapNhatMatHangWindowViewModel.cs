using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public partial class CapNhatMatHangWindowViewModel :
        ObservableObject,
        IRecipient<SelectedIdMessage>
    {
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;
        private int _matHangId;

        public CapNhatMatHangWindowViewModel(
            IMatHangService matHangService,
            IDonViTinhService donViTinhService)
        {
            _matHangService = matHangService;
            _donViTinhService = donViTinhService;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(SelectedIdMessage message)
        {
            _matHangId = message.Value;
            // Load data
            _ = LoadDataAsync();
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maMatHang = "";

        [ObservableProperty]
        private string _tenMatHang = "";

        [ObservableProperty]
        private int _soLuongTon;

        [ObservableProperty]
        private ObservableCollection<DonViTinh> _donViTinhs = [];

        [ObservableProperty]
        private DonViTinh _selectedDonViTinh = null!;

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

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<CapNhatMatHangWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task CapNhatMatHang()
        {
            if (string.IsNullOrWhiteSpace(TenMatHang))
            {
                MessageBox.Show("Tên mặt hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedDonViTinh == null)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}