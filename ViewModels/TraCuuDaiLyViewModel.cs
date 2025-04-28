using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace QuanLyDaiLy.ViewModels
{
    public class TraCuuDaiLyViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _daiLyService;

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

            // Initialize commands
            TraCuuDaiLyCommand = new RelayCommand(async () => await SearchDaiLy());
            CloseCommand = new RelayCommand(CloseWindow);

            _ = LoadDataAsync();
        }

        #region
        private string _maDaiLy = string.Empty;
        public string MaDaiLy
        {
            get => _maDaiLy;
            set
            {
                _maDaiLy = value;
                OnPropertyChanged();
            }
        }

        private string _tenDaiLy = string.Empty;
        public string TenDaiLy
        {
            get => _tenDaiLy;
            set
            {
                _tenDaiLy = value;
                OnPropertyChanged();
            }
        }

        private string _dienThoai = string.Empty;
        public string DienThoai
        {
            get => _dienThoai;
            set
            {
                _dienThoai = value;
                OnPropertyChanged();
            }
        }

        private string _diaChi = string.Empty;
        public string DiaChi
        {
            get => _diaChi;
            set
            {
                _diaChi = value;
                OnPropertyChanged();
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        // LoaiDaiLy related properties
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];
        public ObservableCollection<LoaiDaiLy> LoaiDaiLies
        {
            get => _loaiDaiLies;
            set
            {
                _loaiDaiLies = value;
                OnPropertyChanged();
            }
        }

        private LoaiDaiLy _selectedLoaiDaiLy = new();
        public LoaiDaiLy SelectedLoaiDaiLy
        {
            get => _selectedLoaiDaiLy;
            set
            {
                _selectedLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        // Quan related properties
        private ObservableCollection<Quan> _quans = [];
        public ObservableCollection<Quan> Quans
        {
            get => _quans;
            set
            {
                _quans = value;
                OnPropertyChanged();
            }
        }

        private Quan _selectedQuan = new();
        public Quan SelectedQuan
        {
            get => _selectedQuan;
            set
            {
                _selectedQuan = value;
                OnPropertyChanged();
            }
        }

        // Date range properties
        private DateTime _ngayTiepNhanFrom = DateTime.MinValue;
        public DateTime NgayTiepNhanFrom
        {
            get => _ngayTiepNhanFrom;
            set
            {
                _ngayTiepNhanFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayTiepNhanTo = DateTime.Now;
        public DateTime NgayTiepNhanTo
        {
            get => _ngayTiepNhanTo;
            set
            {
                _ngayTiepNhanTo = value;
                OnPropertyChanged();
            }
        }

        // TienNo range properties
        private long _noDaiLyFrom = 0;
        public long NoDaiLyFrom
        {
            get => _noDaiLyFrom;
            set
            {
                _noDaiLyFrom = value;
                OnPropertyChanged();
            }
        }

        private long _noDaiLyTo = long.MaxValue;
        public long NoDaiLyTo
        {
            get => _noDaiLyTo;
            set
            {
                _noDaiLyTo = value;
                OnPropertyChanged();
            }
        }

        // NoToiDa range properties
        private int _noTheoToiDaLoaiDaiLyFrom = 0;
        public int NoTheoToiDaLoaiDaiLyFrom
        {
            get => _noTheoToiDaLoaiDaiLyFrom;
            set
            {
                _noTheoToiDaLoaiDaiLyFrom = value;
                OnPropertyChanged();
            }
        }

        private int _noTheoToiDaLoaiDaiLyTo = int.MaxValue;
        public int NoTheoToiDaLoaiDaiLyTo
        {
            get => _noTheoToiDaLoaiDaiLyTo;
            set
            {
                _noTheoToiDaLoaiDaiLyTo = value;
                OnPropertyChanged();
            }
        }

        // PhieuXuat related properties
        private int _maPhieuXuatFrom = 0;
        public int MaPhieuXuatFrom
        {
            get => _maPhieuXuatFrom;
            set
            {
                _maPhieuXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private int _maPhieuXuatTo = int.MaxValue;
        public int MaPhieuXuatTo
        {
            get => _maPhieuXuatTo;
            set
            {
                _maPhieuXuatTo = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayLapPhieuXuatFrom = DateTime.MinValue;
        public DateTime NgayLapPhieuXuatFrom
        {
            get => _ngayLapPhieuXuatFrom;
            set
            {
                _ngayLapPhieuXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayLapPhieuXuatTo = DateTime.Now;
        public DateTime NgayLapPhieuXuatTo
        {
            get => _ngayLapPhieuXuatTo;
            set
            {
                _ngayLapPhieuXuatTo = value;
                OnPropertyChanged();
            }
        }

        private long _tongGiaTriPhieuXuatFrom = 0;
        public long TongGiaTriPhieuXuatFrom
        {
            get => _tongGiaTriPhieuXuatFrom;
            set
            {
                _tongGiaTriPhieuXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private long _tongGiaTriPhieuXuatTo = long.MaxValue;
        public long TongGiaTriPhieuXuatTo
        {
            get => _tongGiaTriPhieuXuatTo;
            set
            {
                _tongGiaTriPhieuXuatTo = value;
                OnPropertyChanged();
            }
        }

        // MatHang related properties
        private ObservableCollection<MatHang> _matHangXuats = [];
        public ObservableCollection<MatHang> MatHangXuats
        {
            get => _matHangXuats;
            set
            {
                _matHangXuats = value;
                OnPropertyChanged();
            }
        }

        private MatHang _selectedMatHangXuat = new();
        public MatHang SelectedMatHangXuat
        {
            get => _selectedMatHangXuat;
            set
            {
                _selectedMatHangXuat = value;
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

        // DonViTinh related properties
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

        // SoLuongXuat range properties
        private int _soLuongXuatCuaMatHangXuatFrom = 0;
        public int SoLuongXuatCuaMatHangXuatFrom
        {
            get => _soLuongXuatCuaMatHangXuatFrom;
            set
            {
                _soLuongXuatCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private int _soLuongXuatCuaMatHangXuatTo = int.MaxValue;
        public int SoLuongXuatCuaMatHangXuatTo
        {
            get => _soLuongXuatCuaMatHangXuatTo;
            set
            {
                _soLuongXuatCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        // DonGia range properties
        private long _donGiaXuatCuaMatHangXuatFrom = 0;
        public long DonGiaXuatCuaMatHangXuatFrom
        {
            get => _donGiaXuatCuaMatHangXuatFrom;
            set
            {
                _donGiaXuatCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private long _donGiaXuatCuaMatHangXuatTo = long.MaxValue;
        public long DonGiaXuatCuaMatHangXuatTo
        {
            get => _donGiaXuatCuaMatHangXuatTo;
            set
            {
                _donGiaXuatCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        // ThanhTien range properties
        private long _thanhTienCuaMatHangXuatFrom = 0;
        public long ThanhTienCuaMatHangXuatFrom
        {
            get => _thanhTienCuaMatHangXuatFrom;
            set
            {
                _thanhTienCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private long _thanhTienCuaMatHangXuatTo = long.MaxValue;
        public long ThanhTienCuaMatHangXuatTo
        {
            get => _thanhTienCuaMatHangXuatTo;
            set
            {
                _thanhTienCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        // SoLuongTon range properties
        private int _soLuongTonCuaMatHangXuatFrom = 0;
        public int SoLuongTonCuaMatHangXuatFrom
        {
            get => _soLuongTonCuaMatHangXuatFrom;
            set
            {
                _soLuongTonCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private int _soLuongTonCuaMatHangXuatTo = int.MaxValue;
        public int SoLuongTonCuaMatHangXuatTo
        {
            get => _soLuongTonCuaMatHangXuatTo;
            set
            {
                _soLuongTonCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        // Search Results
        private ObservableCollection<DaiLy> _searchResults = [];
        public ObservableCollection<DaiLy> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }
        #endregion

        // Commands
        public ICommand TraCuuDaiLyCommand { get; }
        public ICommand CloseCommand { get; }

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

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TraCuuDaiLyWindow>().FirstOrDefault()?.Close();
        }

        // services 
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IQuanService _quanService;
        private readonly IDonViTinhService _donViTinhService;
        private readonly IMatHangService _matHangService;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<ObservableCollection<DaiLy>>? SearchCompleted;

        private void ApplySearchResults()
        {
            // Trigger the event with current search results
            SearchCompleted?.Invoke(this, SearchResults);

            // Close the window after applying
            CloseWindow();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}