using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Views.MatHangViews;
using QuanLyDaiLy.Views.PhieuXuatViews;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;


namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public partial class TraCuuPhieuXuatWindowViewModel : ObservableObject
    {
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IDaiLyService _daiLyService;
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IQuanService _quanService;
        private readonly IMatHangService _matHangService;
        private readonly IDonViTinhService _donViTinhService;

        public TraCuuPhieuXuatWindowViewModel(
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService,
            ILoaiDaiLyService loaiDaiLyService,
            IQuanService quanService,
            IMatHangService matHangService,
            IDonViTinhService donViTinhService
            )
        {
            _daiLyService = daiLyService;
            _phieuXuatService = phieuXuatService;
            _loaiDaiLyService = loaiDaiLyService;
            _quanService = quanService;
            _matHangService = matHangService;
            _donViTinhService = donViTinhService;

            _ = LoadDataAsync();
        }

        [ObservableProperty]
        private string _maPhieuXuat = string.Empty;

        [ObservableProperty]
        private ObservableCollection<DaiLy> _daiLies = new ObservableCollection<DaiLy>();

        [ObservableProperty]
        public string _tenDaiLy = string.Empty;

        [ObservableProperty]
        private string _dienThoai = string.Empty;

        [ObservableProperty]
        private string _diaChi = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = new ObservableCollection<LoaiDaiLy>();

        [ObservableProperty]
        private ObservableCollection<Quan> _quans = new ObservableCollection<Quan>();

        [ObservableProperty]
        private Quan _selectedQuans = new();

        [ObservableProperty]
        private DateTime _ngayTiepNhanFrom = DateTime.MinValue;

        [ObservableProperty]
        private DateTime _ngayTiepNhanTo = DateTime.Now;

        [ObservableProperty]
        private string _noDaiLyFrom = string.Empty;

        [ObservableProperty]
        private string _noDaiLyTo = string.Empty;

        [ObservableProperty]
        private string _tongGiaTriPhieuXuatFrom = string.Empty;

        [ObservableProperty]
        private string _tongGiaTriPhieuXuatTo = string.Empty;

        [ObservableProperty]
        private DateTime _ngayLapPhieuXuatFrom = DateTime.MinValue;

        [ObservableProperty]
        private DateTime _ngayLapPhieuXuatTo = DateTime.Now;

        [ObservableProperty]
        private ObservableCollection<MatHang> _matHangXuats = [];

        [ObservableProperty]
        private ObservableCollection<DonViTinh> _donViTinhs= [];

        [ObservableProperty]
        private string _donGiaXuatCuaMatHangXuatFrom = string.Empty;

        [ObservableProperty]
        private string _donGiaXuatCuaMatHangXuatTo = string.Empty;

        [ObservableProperty]
        private string _soLuongXuatCuaMatHangXuatFrom = string.Empty;

        [ObservableProperty]
        private string _soLuongXuatCuaMatHangXuatTo = string.Empty;

        [ObservableProperty]
        private string _soLuongTonCuaMatHangXuatFrom = string.Empty;

        [ObservableProperty]
        private string _soLuongTonCuaMatHangXuatTo = string.Empty;

        [ObservableProperty]
        private string _thanhTienCuaMatHangXuatFrom = string.Empty;

        [ObservableProperty]
        private string _thanhTienCuaMatHangXuatTo = string.Empty;

        [ObservableProperty]
        private DaiLy _selectedDaiLies = new();

        [ObservableProperty]
        private LoaiDaiLy _selectedLoaiDaiLies = new();

        [ObservableProperty]
        private MatHang _selectedMatHangXuats = new();

        [ObservableProperty]
        private string _tenMatHang = string.Empty;

        [ObservableProperty]
        private DonViTinh _selectedDonViTinhs = new();

        [ObservableProperty]
        private string _tenDonViTinh = string.Empty;

        [ObservableProperty]
        private ObservableCollection<PhieuXuat> _searchResults = [];

        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TraCuuPhieuXuatWindow>().FirstOrDefault()?.Close();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var listDaiLy = await _daiLyService.GetAllDaiLy();
                var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
                var listQuan = await _quanService.GetAllQuan();
                var listMatHang = await _matHangService.GetAllMatHang();
                var listDonViTinh = await _donViTinhService.GetAllDonViTinh();

                LoaiDaiLies.Clear();
                DaiLies.Clear();
                Quans.Clear();
                MatHangXuats.Clear();
                DonViTinhs.Clear();


                // Populate the collections
                LoaiDaiLies = new ObservableCollection<LoaiDaiLy>(listLoaiDaiLy);
                DaiLies = new ObservableCollection<DaiLy>(listDaiLy);
                Quans = new ObservableCollection<Quan>(listQuan);
                MatHangXuats = new ObservableCollection<MatHang>(listMatHang);
                DonViTinhs = new ObservableCollection<DonViTinh>(listDonViTinh);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SearchPhieuXuat()
        {
            try
            {
                var phieuXuats = await _phieuXuatService.GetAllPhieuXuat();

                if (!string.IsNullOrEmpty(MaPhieuXuat))
                {
                    phieuXuats = phieuXuats.Where(px => px.MaPhieuXuat.ToString().Contains(MaPhieuXuat));
                }

                if (SelectedDaiLies.MaDaiLy != 0)
                {
                    phieuXuats = phieuXuats.Where(px => px.MaDaiLy == SelectedDaiLies.MaDaiLy);
                }

                if (!string.IsNullOrEmpty(DienThoai))
                {
                    phieuXuats = phieuXuats.Where(px => px.DaiLy.DienThoai.Contains(DienThoai));
                }

                if (!string.IsNullOrEmpty(DiaChi))
                {
                    phieuXuats = phieuXuats.Where(px => px.DaiLy.DiaChi.Contains(DiaChi));
                }

                if (!string.IsNullOrEmpty(Email))
                {
                    phieuXuats = phieuXuats.Where(px => px.DaiLy.Email.Contains(Email));
                }

                if (SelectedLoaiDaiLies.MaLoaiDaiLy != 0)
                {
                    phieuXuats = phieuXuats.Where(px => px.DaiLy.MaLoaiDaiLy == SelectedLoaiDaiLies.MaLoaiDaiLy);
                }

                if (SelectedQuans.MaQuan != 0)
                {
                    phieuXuats = phieuXuats.Where(px => px.DaiLy.MaQuan == SelectedQuans.MaQuan);
                }
                                
                if (NgayTiepNhanFrom != DateTime.MinValue && NgayTiepNhanTo != DateTime.MinValue)
                {
                    phieuXuats = phieuXuats.Where(d => d.DaiLy.NgayTiepNhan >= NgayTiepNhanFrom && d.DaiLy.NgayTiepNhan <= NgayTiepNhanTo);
                }
                else
                {
                    if (NgayTiepNhanFrom != DateTime.MinValue)
                    {

                        phieuXuats = phieuXuats.Where(d => d.DaiLy.NgayTiepNhan >= NgayTiepNhanFrom);
                    }
                    if (NgayTiepNhanTo != DateTime.MinValue)
                    {

                        phieuXuats = phieuXuats.Where(d => d.DaiLy.NgayTiepNhan <= NgayTiepNhanTo);
                    }
                }
                
                if (!string.IsNullOrEmpty(NoDaiLyFrom) && !string.IsNullOrEmpty(NoDaiLyTo)
                    && long.TryParse(NoDaiLyFrom, out var fromNo) && long.TryParse(NoDaiLyTo, out var toNo))
                {
                    phieuXuats = phieuXuats.Where(d => d.DaiLy.TienNo >= fromNo && d.DaiLy.TienNo <= toNo);
                }
                else
                {
                    if (!string.IsNullOrEmpty(NoDaiLyFrom) && long.TryParse(NoDaiLyFrom, out fromNo))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DaiLy.TienNo >= fromNo);
                    }
                    if (!string.IsNullOrEmpty(NoDaiLyTo) && long.TryParse(NoDaiLyTo, out toNo))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DaiLy.TienNo <= toNo);
                    }
                }                
                
                if (NgayLapPhieuXuatFrom != DateTime.MinValue && NgayLapPhieuXuatTo != DateTime.MinValue)
                {
                    phieuXuats = phieuXuats.Where(d => d.NgayLapPhieu >= NgayLapPhieuXuatFrom && d.NgayLapPhieu <= NgayLapPhieuXuatTo);
                }
                else
                {
                    
                    if (NgayLapPhieuXuatFrom != DateTime.MinValue)
                    {
                        phieuXuats = phieuXuats.Where(d => d.NgayLapPhieu >= NgayLapPhieuXuatFrom);
                    }
                    if (NgayLapPhieuXuatTo != DateTime.MinValue)
                    {
                        phieuXuats = phieuXuats.Where(d => d.NgayLapPhieu <= NgayLapPhieuXuatTo);
                    }
                }
                
                if (!string.IsNullOrEmpty(TongGiaTriPhieuXuatFrom) && !string.IsNullOrEmpty(TongGiaTriPhieuXuatTo)
                    && long.TryParse(TongGiaTriPhieuXuatFrom, out var fromTongGiaTri) && long.TryParse(TongGiaTriPhieuXuatTo, out var toTongGiaTri))
                {
                    phieuXuats = phieuXuats.Where(d => d.TongTriGia >= fromTongGiaTri && d.TongTriGia <= toTongGiaTri);
                }
                else
                {
                    if (!string.IsNullOrEmpty(TongGiaTriPhieuXuatFrom) && long.TryParse(TongGiaTriPhieuXuatFrom, out fromTongGiaTri))
                    {
                        phieuXuats = phieuXuats.Where(d => d.TongTriGia >= fromTongGiaTri);
                    }
                    if (!string.IsNullOrEmpty(TongGiaTriPhieuXuatTo) && long.TryParse(TongGiaTriPhieuXuatTo, out toTongGiaTri))
                    {
                        phieuXuats = phieuXuats.Where(d => d.TongTriGia <= toTongGiaTri);
                    }
                }

                if (SelectedMatHangXuats.MaMatHang != 0)
                {
                    phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.MaMatHang == SelectedMatHangXuats.MaMatHang));

                }

                if (SelectedDonViTinhs.MaDonViTinh != 0)
                {

                    if (SelectedDonViTinhs.MaDonViTinh != 0)
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.MaDonViTinh == SelectedDonViTinhs.MaDonViTinh));
                    }
                }
                
                if (!string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatFrom) && !string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatTo)
                    && int.TryParse(SoLuongXuatCuaMatHangXuatFrom, out var fromSoLuong) && int.TryParse(SoLuongXuatCuaMatHangXuatTo, out var toSoLuong))
                {
                    phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat >= fromSoLuong && ct.SoLuongXuat <= toSoLuong));
                }
                else
                {
                    if (!string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatFrom) && int.TryParse(SoLuongXuatCuaMatHangXuatFrom, out fromSoLuong))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat >= fromSoLuong));
                    }
                    if (!string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatTo) && int.TryParse(SoLuongXuatCuaMatHangXuatTo, out toSoLuong))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat <= toSoLuong));
                    }
                }
                
                if (!string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatFrom) && !string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatTo)
                    && long.TryParse(DonGiaXuatCuaMatHangXuatFrom, out var fromDonGia) && long.TryParse(DonGiaXuatCuaMatHangXuatTo, out var toDonGia))
                {
                    phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.DonGia >= fromDonGia && ct.DonGia <= toDonGia));
                }
                else
                {
                    if (!string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatFrom) && long.TryParse(DonGiaXuatCuaMatHangXuatFrom, out fromDonGia))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.DonGia >= fromDonGia));
                    }
                    if (!string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatTo) && long.TryParse(DonGiaXuatCuaMatHangXuatTo, out toDonGia))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.DonGia <= toDonGia));
                    }
                }
                
                if (!string.IsNullOrEmpty(ThanhTienCuaMatHangXuatFrom) && !string.IsNullOrEmpty(ThanhTienCuaMatHangXuatTo)
                    && int.TryParse(ThanhTienCuaMatHangXuatFrom, out var fromThanhTien) && int.TryParse(ThanhTienCuaMatHangXuatTo, out var toThanhTien))
                {
                    phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien >= fromThanhTien && ct.ThanhTien <= toThanhTien));
                }
                else
                {
                    if (!string.IsNullOrEmpty(ThanhTienCuaMatHangXuatFrom) && int.TryParse(ThanhTienCuaMatHangXuatFrom, out fromThanhTien))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien >= fromThanhTien));
                    }
                    if (!string.IsNullOrEmpty(ThanhTienCuaMatHangXuatTo) && int.TryParse(ThanhTienCuaMatHangXuatTo, out toThanhTien))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien <= toThanhTien));
                    }
                }
                
                if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom) && !string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo)
                    && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out var fromSoLuongTon) && int.TryParse(SoLuongTonCuaMatHangXuatTo, out var toSoLuongTon))
                {
                    phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon >= fromSoLuongTon && ct.MatHang.SoLuongTon <= toSoLuongTon));
                }
                else
                {
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom) && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out fromSoLuongTon))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon >= fromSoLuongTon));
                    }
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo) && int.TryParse(SoLuongTonCuaMatHangXuatTo, out toSoLuongTon))
                    {
                        phieuXuats = phieuXuats.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon <= toSoLuongTon));
                    }
                }

                SearchResults = [.. phieuXuats];
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

            WeakReferenceMessenger.Default.Send(new SearchCompletedMessage<PhieuXuat>(SearchResults));
           // Close the window after applying PhieuXuat
            CloseWindow();
        }        
    }
}
