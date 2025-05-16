using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.PhieuThuViews;

namespace QuanLyDaiLy.ViewModels.PhieuThuViewModels
{
    public partial class TraCuuPhieuThuWindowViewModel : ObservableObject
    {
        // Services
        private IPhieuThuService _phieuThuService;
        private IQuanService _quanService;
        private IDaiLyService _daiLyService;
        private ILoaiDaiLyService _loaiDaiLyService;

        public TraCuuPhieuThuWindowViewModel(
            IPhieuThuService phieuThuService,
            IQuanService quanService,
            IDaiLyService daiLyService,
            ILoaiDaiLyService loaiDaiLyService
        )
        {
            // Initialize services
            _phieuThuService = phieuThuService;
            _quanService = quanService;
            _daiLyService = daiLyService;
            _loaiDaiLyService = loaiDaiLyService;

            _ = LoadData();
        }
        private async Task LoadData()
        {
            try
            {
                var listDaiLy = await _daiLyService.GetAllDaiLy();
                var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
                var listQuan = await _quanService.GetAllQuan();

                DaiLies.Clear();
                LoaiDaiLies.Clear();
                Quans.Clear();

                DaiLies = [.. listDaiLy];
                LoaiDaiLies = [.. listLoaiDaiLy];
                Quans = [.. listQuan];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Binding Properties
        [ObservableProperty]
        private ObservableCollection<DaiLy> _daiLies = [];
        [ObservableProperty]
        private DaiLy _selectedDaiLy = new();
        [ObservableProperty]
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];
        [ObservableProperty]
        private LoaiDaiLy _selectedLoaiDaiLy = new();
        [ObservableProperty]
        private ObservableCollection<Quan> _quans = [];
        [ObservableProperty]
        private Quan _selectedQuan = new();
        [ObservableProperty]
        private string _maPhieuThu = string.Empty;
        [ObservableProperty]
        private string _dienThoai = string.Empty;
        [ObservableProperty]
        private string _diaChi = string.Empty;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private DateTime _ngayTiepNhanFrom = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _ngayTiepNhanTo = DateTime.Now;
        [ObservableProperty]
        private DateTime _ngayThuTienFrom = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _ngayThuTienTo = DateTime.Now;
        [ObservableProperty]
        private long _noDaiLyFrom = 0;
        [ObservableProperty]
        private long _noDaiLyTo = long.MaxValue;
        [ObservableProperty]
        private long _soTienThuFrom = 0;
        [ObservableProperty]
        private long _soTienThuTo = long.MaxValue;
        [ObservableProperty]
        private ObservableCollection<PhieuThu> _searchResults = [];
        #endregion

        #region RelayCommand
        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TraCuuPhieuThuTienWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task SearchPhieuThu()
        {
            try
            {
                var phieuThus = await _phieuThuService.GetAllPhieuThu();
                ObservableCollection<PhieuThu> filteredResults = [.. phieuThus];

                if (!string.IsNullOrEmpty(MaPhieuThu))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaPhieuThu.ToString().Contains(MaPhieuThu))];
                }
                if (!string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaDaiLy == SelectedDaiLy.MaDaiLy)];
                }
                if (!string.IsNullOrEmpty(DienThoai))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.DienThoai.Contains(DienThoai))];
                }
                if (!string.IsNullOrEmpty(DiaChi))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.DiaChi.Contains(DiaChi))];
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.Email.Contains(Email))];
                }
                if (!string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.MaLoaiDaiLy == SelectedLoaiDaiLy.MaLoaiDaiLy)];
                }
                if (!string.IsNullOrEmpty(SelectedQuan.TenQuan))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.MaQuan == SelectedQuan.MaQuan)];
                }
                if (NgayTiepNhanFrom != DateTime.MinValue && NgayTiepNhanTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.NgayTiepNhan >= NgayTiepNhanFrom && d.DaiLy.NgayTiepNhan <= NgayTiepNhanTo)];
                }
                if (NgayThuTienFrom != DateTime.MinValue && NgayThuTienTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.NgayThuTien >= NgayThuTienFrom && d.NgayThuTien <= NgayThuTienTo)];
                }
                if (NoDaiLyFrom != 0 || NoDaiLyTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.TienNo >= NoDaiLyFrom && d.DaiLy.TienNo <= NoDaiLyTo)];
                }
                if (SoTienThuFrom != 0 || SoTienThuTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.SoTienThu >= SoTienThuFrom && d.SoTienThu <= SoTienThuTo)];
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
            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<PhieuThu>(SearchResults));
            CloseWindow();
        }

        #endregion
    }
}
