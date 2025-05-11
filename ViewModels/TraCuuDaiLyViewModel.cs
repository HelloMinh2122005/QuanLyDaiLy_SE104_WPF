using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace QuanLyDaiLy.ViewModels
{
    public partial class TraCuuDaiLyViewModel : ObservableObject
    {
        // Services
        private readonly IDaiLyService _daiLyService;
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IQuanService _quanService;
        private readonly IDonViTinhService _donViTinhService;
        private readonly IMatHangService _matHangService;

        public TraCuuDaiLyViewModel(
            ILoaiDaiLyService loaiDaiLyService,
            IQuanService quanService,
            IDonViTinhService donViTinhService,
            IMatHangService matHangService,
            IDaiLyService daiLyService
        )
        {
            _loaiDaiLyService = loaiDaiLyService;
            _quanService = quanService;
            _donViTinhService = donViTinhService;
            _matHangService = matHangService;
            _daiLyService = daiLyService;

            _ = LoadDataAsync();
        }

        #region Binding Properties
        [ObservableProperty]
        private string _maDaiLy = string.Empty;
        [ObservableProperty]
        private string _tenDaiLy = string.Empty;
        [ObservableProperty]
        private string _dienThoai = string.Empty;
        [ObservableProperty]
        private string _diaChi = string.Empty;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];
        [ObservableProperty]
        private LoaiDaiLy _selectedLoaiDaiLy = new();
        [ObservableProperty]
        private ObservableCollection<Quan> _quans = [];
        [ObservableProperty]
        private Quan _selectedQuan = new();
        [ObservableProperty]
        private DateTime _ngayTiepNhanFrom = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _ngayTiepNhanTo = DateTime.Now;
        [ObservableProperty]
        private long _noDaiLyFrom = 0;
        [ObservableProperty]
        private long _noDaiLyTo = long.MaxValue;
        [ObservableProperty]
        private int _noTheoToiDaLoaiDaiLyFrom = 0;
        [ObservableProperty]
        private int _noTheoToiDaLoaiDaiLyTo = int.MaxValue;
        [ObservableProperty]
        private int _maPhieuXuatFrom = 0;
        [ObservableProperty]
        private int _maPhieuXuatTo = int.MaxValue;
        [ObservableProperty]
        private DateTime _ngayLapPhieuXuatFrom = DateTime.MinValue;
        [ObservableProperty]
        private DateTime _ngayLapPhieuXuatTo = DateTime.Now;
        [ObservableProperty]
        private long _tongGiaTriPhieuXuatFrom = 0;
        [ObservableProperty]
        private long _tongGiaTriPhieuXuatTo = long.MaxValue;
        [ObservableProperty]
        private ObservableCollection<MatHang> _matHangXuats = [];
        [ObservableProperty]
        private MatHang _selectedMatHangXuat = new();
        [ObservableProperty]
        private string _tenMatHang = string.Empty;
        [ObservableProperty]
        private ObservableCollection<DonViTinh> _donViTinhs = [];
        [ObservableProperty]
        private DonViTinh _selectedDonViTinh = new();
        [ObservableProperty]
        private string _tenDonViTinh = string.Empty;
        [ObservableProperty]
        private int _soLuongXuatCuaMatHangXuatFrom = 0;
        [ObservableProperty]
        private int _soLuongXuatCuaMatHangXuatTo = int.MaxValue;
        [ObservableProperty]
        private long _donGiaXuatCuaMatHangXuatFrom = 0;
        [ObservableProperty]
        private long _donGiaXuatCuaMatHangXuatTo = long.MaxValue;
        [ObservableProperty]
        private long _thanhTienCuaMatHangXuatFrom = 0;
        [ObservableProperty]
        private long _thanhTienCuaMatHangXuatTo = long.MaxValue;
        [ObservableProperty]
        private int _soLuongTonCuaMatHangXuatFrom = 0;
        [ObservableProperty]
        private int _soLuongTonCuaMatHangXuatTo = int.MaxValue;
        // Search Results
        public ObservableCollection<DaiLy> SearchResults = [];
        #endregion

        private async Task LoadDataAsync()
        {
            try
            {
                var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
                var listQuan = await _quanService.GetAllQuan();
                var listDonViTinh = await _donViTinhService.GetAllDonViTinh();
                var listMatHang = await _matHangService.GetAllMatHang();

                LoaiDaiLies.Clear();
                Quans.Clear();
                DonViTinhs.Clear();
                MatHangXuats.Clear();

                LoaiDaiLies = [.. listLoaiDaiLy];
                Quans = [.. listQuan];
                DonViTinhs = [.. listDonViTinh];
                MatHangXuats = [.. listMatHang];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region RelayCommands
        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TraCuuDaiLyWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task SearchDaiLy()
        {
            try
            {
                var daiLies = await _daiLyService.GetAllDaiLy();
                ObservableCollection<DaiLy> filteredResults = [.. daiLies];

                if (!string.IsNullOrEmpty(MaDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaDaiLy.ToString().Contains(MaDaiLy))];
                }
                if (!string.IsNullOrEmpty(TenDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.TenDaiLy.Contains(TenDaiLy))];
                }
                if (!string.IsNullOrEmpty(DienThoai))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DienThoai.Contains(DienThoai))];
                }
                if (!string.IsNullOrEmpty(DiaChi))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DiaChi.Contains(DiaChi))];
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    filteredResults = [.. filteredResults.Where(d => d.Email.Contains(Email))];
                }
                if (!string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaLoaiDaiLy == SelectedLoaiDaiLy.MaLoaiDaiLy)];
                }
                if (!string.IsNullOrEmpty(SelectedQuan.TenQuan))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaQuan == SelectedQuan.MaQuan)];
                }
                // Tìm kiếm theo ngày tiếp nhận (từ - đến)
                if (NgayTiepNhanFrom != DateTime.MinValue && NgayTiepNhanTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.NgayTiepNhan >= NgayTiepNhanFrom && d.NgayTiepNhan <= NgayTiepNhanTo)];
                }
                // Tìm kiếm theo tiền nợ (từ - đến)
                if (NoDaiLyFrom != 0 || NoDaiLyTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.TienNo >= NoDaiLyFrom && d.TienNo <= NoDaiLyTo)];
                }
                // Tìm kiếm theo nợ tối đa (từ - đến)
                if (NoTheoToiDaLoaiDaiLyFrom != 0 || NoTheoToiDaLoaiDaiLyTo != int.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.LoaiDaiLy.NoToiDa >= NoTheoToiDaLoaiDaiLyFrom && d.LoaiDaiLy.NoToiDa <= NoTheoToiDaLoaiDaiLyTo)];
                }
                // Tìm kiếm theo mã phiếu xuất (từ - đến)
                if (MaPhieuXuatFrom != 0 || MaPhieuXuatTo != int.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.MaPhieuXuat >= MaPhieuXuatFrom && p.MaPhieuXuat <= MaPhieuXuatTo))];
                }
                // Tìm kiếm theo ngày lập phiếu xuất (từ - đến)
                if (NgayLapPhieuXuatFrom != DateTime.MinValue && NgayLapPhieuXuatTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.NgayLapPhieu >= NgayLapPhieuXuatFrom && p.NgayLapPhieu <= NgayLapPhieuXuatTo))];
                }
                // Tìm kiếm theo tổng giá trị phiếu xuất (từ - đến)
                if (TongGiaTriPhieuXuatFrom != 0 || TongGiaTriPhieuXuatTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.TongTriGia >= TongGiaTriPhieuXuatFrom && p.TongTriGia <= TongGiaTriPhieuXuatTo))];
                }
                // Tìm kiếm theo mặt hàng xuất
                if (SelectedMatHangXuat.MaMatHang != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.DsChiTietPhieuXuat.Any(ct => ct.MatHang.MaMatHang == SelectedMatHangXuat.MaMatHang)))];
                }
                // Tìm kiếm theo đơn vị tính
                if (SelectedDonViTinh.MaDonViTinh != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.DsChiTietPhieuXuat.Any(ct => ct.MatHang.MaDonViTinh == SelectedDonViTinh.MaDonViTinh)))];
                }
                // Tìm kiếm theo số lượng xuất (từ - đến)
                if (SoLuongXuatCuaMatHangXuatFrom != 0 || SoLuongXuatCuaMatHangXuatTo != int.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat >= SoLuongXuatCuaMatHangXuatFrom && ct.SoLuongXuat <= SoLuongXuatCuaMatHangXuatTo)))];
                }
                // Tìm kiếm theo đơn giá xuất (từ - đến)
                if (DonGiaXuatCuaMatHangXuatFrom != 0 || DonGiaXuatCuaMatHangXuatTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.DsChiTietPhieuXuat.Any(ct => ct.DonGia >= DonGiaXuatCuaMatHangXuatFrom && ct.DonGia <= DonGiaXuatCuaMatHangXuatTo)))];
                }
                // Tìm kiếm theo thành tiền xuất (từ - đến)
                if (ThanhTienCuaMatHangXuatFrom != 0 || ThanhTienCuaMatHangXuatTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien >= ThanhTienCuaMatHangXuatFrom && ct.ThanhTien <= ThanhTienCuaMatHangXuatTo)))];
                }
                // Tìm kiếm theo số lượng tồn (từ - đến)
                if (SoLuongTonCuaMatHangXuatFrom != 0 || SoLuongTonCuaMatHangXuatTo != int.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsPhieuXuat.Any(p => p.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon >= SoLuongTonCuaMatHangXuatFrom && ct.MatHang.SoLuongTon <= SoLuongTonCuaMatHangXuatTo)))];
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
            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<DaiLy>(SearchResults));
            CloseWindow();
        }
        #endregion
    }
}