using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.MatHangViews;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public partial class TraCuuMatHangWindowViewModel : ObservableObject
    {
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;

        [ObservableProperty]
        private string _maMatHang = string.Empty;

        [ObservableProperty]
        private string _tenMatHang = string.Empty;

        [ObservableProperty]
        private ObservableCollection<DonViTinh> _donViTinhs = [];

        [ObservableProperty]
        private DonViTinh _selectedDonViTinh = null!;

        [ObservableProperty]
        private string _soLuongTonCuaMatHangXuatFrom = string.Empty;

        [ObservableProperty]
        private string _soLuongTonCuaMatHangXuatTo = string.Empty;

        public ObservableCollection<MatHang> SearchResults = [];

        public TraCuuMatHangWindowViewModel(
            IDonViTinhService donViTinhService,
            IMatHangService matHangService
        ) {
            _donViTinhService = donViTinhService;
            _matHangService = matHangService;

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var listDonViTinh = await _donViTinhService.GetAllDonViTinh();
                DonViTinhs = [.. listDonViTinh];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Close()
        {
            Application.Current.Windows.OfType<TraCuuMatHangWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TraCuuMatHang()
        {
            try
            {
                var filteredResults = await _matHangService.GetAllMatHang();

                if (!string.IsNullOrEmpty(MaMatHang))
                {
                    filteredResults = filteredResults.Where(d => d.MaMatHang.ToString().Contains(MaMatHang));
                }

                if (!string.IsNullOrEmpty(TenMatHang))
                {
                    filteredResults = filteredResults.Where(d => d.TenMatHang.Contains(TenMatHang));
                }

                if (SelectedDonViTinh != null! && SelectedDonViTinh.MaDonViTinh != 0)
                {
                    filteredResults = filteredResults.Where(d => d.MaDonViTinh == SelectedDonViTinh.MaDonViTinh);
                }

                // —— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom)
                    && !string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo)
                    && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out var fromQty)
                    && int.TryParse(SoLuongTonCuaMatHangXuatTo, out var toQty))
                {
                    filteredResults = filteredResults.Where(d => d.SoLuongTon >= fromQty && d.SoLuongTon <= toQty);
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom)
                        && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out fromQty))
                    {
                        filteredResults = filteredResults.Where(d => d.SoLuongTon >= fromQty);
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo)
                        && int.TryParse(SoLuongTonCuaMatHangXuatTo, out toQty))
                    {
                        filteredResults = filteredResults.Where(d => d.SoLuongTon <= toQty);
                    }
                }

                SearchResults = [.. filteredResults];

                ApplySearchResults();

                if (SearchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào phù hợp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySearchResults()
        {
            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<MatHang>(SearchResults));
            Close();
        }

    }
}
