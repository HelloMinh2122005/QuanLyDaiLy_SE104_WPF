using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Views.MatHangViews;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public class TraCuuMatHangWindowViewModel : INotifyPropertyChanged
    {
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;

        public TraCuuMatHangWindowViewModel(
            IDonViTinhService donViTinhService,
            IMatHangService matHangService
            )
        {
            _donViTinhService = donViTinhService;
            _matHangService = matHangService;

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            TraCuuMatHangCommand = new RelayCommand(async () => await SearchMatHang());

            _ = LoadDataAsync();
        }

        // properties for binding
        private string _maMatHang = string.Empty;
        public string MaMatHang
        {
            get => _maMatHang;
            set
            {
                _maMatHang = value;
                OnPropertyChanged();
            }
        }

        private string _tenMatHang = string.Empty;
        public string TenMatHang
        {
            get => _tenMatHang;
            set
            {
                _tenMatHang = value;
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

        private DonViTinh _selectedDonViTinh = new();
        public DonViTinh SelectedDonViTinh
        {
            get => _selectedDonViTinh;
            set
            {
                _selectedDonViTinh = value;
                OnPropertyChanged();
            }
        }

        private string _soLuongTonCuaMatHangXuatFrom = string.Empty;
        public string SoLuongTonCuaMatHangXuatFrom
        {
            get => _soLuongTonCuaMatHangXuatFrom;
            set
            {
                _soLuongTonCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private string _soLuongTonCuaMatHangXuatTo = string.Empty;
        public string SoLuongTonCuaMatHangXuatTo
        {
            get => _soLuongTonCuaMatHangXuatTo;
            set
            {
                _soLuongTonCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        // Search Results
        private ObservableCollection<MatHang> _searchResults = [];
        public ObservableCollection<MatHang> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseCommand { get; }
        public ICommand TraCuuMatHangCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                var listDonViTinh = await _donViTinhService.GetAllDonViTinh();

                DonViTinhs.Clear();

                DonViTinhs = [.. listDonViTinh];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TraCuuMatHangWindow>().FirstOrDefault()?.Close();
        }

        private async Task SearchMatHang()
        {
            try
            {
                // 1. Kiểm tra nếu chưa nhập gì hết thì hỏi xác nhận
                bool isFilterEmpty =
                    string.IsNullOrEmpty(MaMatHang) &&
                    string.IsNullOrEmpty(TenMatHang) &&
                    SelectedDonViTinh.MaDonViTinh == 0 &&
                    string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom) &&
                    string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo);

                if (isFilterEmpty)
                {
                    var result = MessageBox.Show(
                        "Bạn chưa nhập thông tin tìm kiếm mặt hàng.\nBạn có chắc muốn tiếp tục tra cứu không?",
                        "Xác nhận",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                        return;  // Dừng hàm, không thực hiện search
                }

                var matHangs = await _matHangService.GetAllMatHang();
                ObservableCollection<MatHang> filteredResults = [.. matHangs];

                if (!string.IsNullOrEmpty(MaMatHang))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaMatHang.ToString().Contains(MaMatHang))];
                }

                if (!string.IsNullOrEmpty(TenMatHang))
                {
                    filteredResults = [.. filteredResults.Where(d => d.TenMatHang.Contains(TenMatHang))];
                }

                if (SelectedDonViTinh.MaDonViTinh != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaDonViTinh == SelectedDonViTinh.MaDonViTinh)];
                }

                // —— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom)
                    && !string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo)
                    && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out var fromQty)
                    && int.TryParse(SoLuongTonCuaMatHangXuatTo, out var toQty))
                {
                    filteredResults = [.. filteredResults
                .Where(d => d.SoLuongTon >= fromQty && d.SoLuongTon <= toQty)];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom)
                        && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out fromQty))
                    {
                        filteredResults = [.. filteredResults
                    .Where(d => d.SoLuongTon >= fromQty)];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo)
                        && int.TryParse(SoLuongTonCuaMatHangXuatTo, out toQty))
                    {
                        filteredResults = [.. filteredResults
                    .Where(d => d.SoLuongTon <= toQty)];
                    }
                }
                // —— hết phần lọc số lượng tồn —— 

                SearchResults = [.. filteredResults];

                // Raise the event with the search results
                SearchCompleted?.Invoke(this, SearchResults);
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
            // Trigger the event with current search results
            SearchCompleted?.Invoke(this, SearchResults);

            // Close the window after applying
            CloseWindow();
        }

        public event EventHandler<ObservableCollection<MatHang>>? SearchCompleted;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
