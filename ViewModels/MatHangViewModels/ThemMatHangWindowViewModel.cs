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
    public partial class ThemMatHangWindowViewModel : ObservableObject
    {
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;

        public ThemMatHangWindowViewModel(IMatHangService matHangService, IDonViTinhService donViTinhService)
        {
            _matHangService = matHangService;
            _donViTinhService = donViTinhService;

            // Load data
            _ = LoadDonViTinh();
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maMatHang = "";

        [ObservableProperty]
        private string _tenMatHang = "";

        [ObservableProperty]
        private int _soLuongTon = 0;

        [ObservableProperty]
        private ObservableCollection<DonViTinh> _donViTinhs = [];

        [ObservableProperty]
        private DonViTinh _selectedDonViTinh = null!;

        private async Task LoadDonViTinh()
        {
            try
            {
                MaMatHang = (await _matHangService.GenerateAvailableId()).ToString();

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

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ThemMatHangWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TiepNhanMatHang()
        {
            if (string.IsNullOrWhiteSpace(TenMatHang))
            {
                MessageBox.Show("Tên mặt hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelectedDonViTinh == null || string.IsNullOrEmpty(SelectedDonViTinh.TenDonViTinh))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                

                MatHang matHang = new()
                {
                    MaMatHang = int.Parse(MaMatHang),
                    TenMatHang = TenMatHang,
                    MaDonViTinh = SelectedDonViTinh.MaDonViTinh,
                    SoLuongTon = SoLuongTon
                };

                await _matHangService.AddMatHang(matHang);
                MessageBox.Show("Thêm mặt hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
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
    }
}