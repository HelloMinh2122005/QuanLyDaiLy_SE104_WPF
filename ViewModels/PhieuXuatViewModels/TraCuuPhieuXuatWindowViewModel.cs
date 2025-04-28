using QuanLyDaiLy.Commands;
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

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public class TraCuuPhieuXuatWindowViewModel : INotifyPropertyChanged
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

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            TraCuuPhieuXuatCommand = new RelayCommand(async () => await SearchPhieuXuat());

            _ = LoadDataAsync();
        }

        // properties for binding
        private string _maPhieuXuat = string.Empty;
        public string MaPhieuXuat
        {
            get => _maPhieuXuat;
            set
            {
                _maPhieuXuat = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DaiLy> _daiLies = [];
        public ObservableCollection<DaiLy> DaiLies
        {
            get => _daiLies;
            set
            {
                _daiLies = value;
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

        private Quan _selectedQuans = new();
        public Quan SelectedQuans
        {
            get => _selectedQuans;
            set
            {
                _selectedQuans = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayTiepNhanFrom;
        public DateTime NgayTiepNhanFrom
        {
            get => _ngayTiepNhanFrom;
            set
            {
                _ngayTiepNhanFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayTiepNhanTo;
        public DateTime NgayTiepNhanTo
        {
            get => _ngayTiepNhanTo;
            set
            {
                _ngayTiepNhanTo = value;
                OnPropertyChanged();
            }
        }

        private string _noDaiLyFrom = string.Empty;
        public string NoDaiLyFrom
        {
            get => _noDaiLyFrom;
            set
            {
                _noDaiLyFrom = value;
                OnPropertyChanged();
            }
        }

        private string _noDaiLyTo = string.Empty;
        public string NoDaiLyTo
        {
            get => _noDaiLyTo;
            set
            {
                _noDaiLyTo = value;
                OnPropertyChanged();
            }
        }

        private string _tongGiaTriPhieuXuatFrom = string.Empty;
        public string TongGiaTriPhieuXuatFrom
        {
            get => _tongGiaTriPhieuXuatFrom;
            set
            {
                _tongGiaTriPhieuXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private string _tongGiaTriPhieuXuatTo = string.Empty;
        public string TongGiaTriPhieuXuatTo
        {
            get => _tongGiaTriPhieuXuatTo;
            set
            {
                _tongGiaTriPhieuXuatTo = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayLapPhieuXuatFrom;
        public DateTime NgayLapPhieuXuatFrom
        {
            get => _ngayLapPhieuXuatFrom;
            set
            {
                _ngayLapPhieuXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayLapPhieuXuatTo;
        public DateTime NgayLapPhieuXuatTo
        {
            get => _ngayLapPhieuXuatTo;
            set
            {
                _ngayLapPhieuXuatTo = value;
                OnPropertyChanged();
            }
        }

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

        private ObservableCollection<DonViTinh> _donViTinhs= [];
        public ObservableCollection<DonViTinh> DonViTinhs
        {
            get => _donViTinhs;
            set
            {
                _donViTinhs = value;
                OnPropertyChanged();
            }
        }

        private string _donGiaXuatCuaMatHangXuatFrom = string.Empty;
        public string DonGiaXuatCuaMatHangXuatFrom
        {
            get => _donGiaXuatCuaMatHangXuatFrom;
            set
            {
                _donGiaXuatCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private string _donGiaXuatCuaMatHangXuatTo = string.Empty;
        public string DonGiaXuatCuaMatHangXuatTo
        {
            get => _donGiaXuatCuaMatHangXuatTo;
            set
            {
                _donGiaXuatCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        private string _soLuongXuatCuaMatHangXuatFrom = string.Empty;
        public string SoLuongXuatCuaMatHangXuatFrom
        {
            get => _soLuongXuatCuaMatHangXuatFrom;
            set
            {
                _soLuongXuatCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private string _soLuongXuatCuaMatHangXuatTo = string.Empty;
        public string SoLuongXuatCuaMatHangXuatTo
        {
            get => _soLuongXuatCuaMatHangXuatTo;
            set
            {
                _soLuongXuatCuaMatHangXuatTo = value;
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

        private string _thanhTienCuaMatHangXuatFrom = string.Empty;
        public string ThanhTienCuaMatHangXuatFrom
        {
            get => _thanhTienCuaMatHangXuatFrom;
            set
            {
                _thanhTienCuaMatHangXuatFrom = value;
                OnPropertyChanged();
            }
        }

        private string _thanhTienCuaMatHangXuatTo = string.Empty;
        public string ThanhTienCuaMatHangXuatTo
        {
            get => _thanhTienCuaMatHangXuatTo;
            set
            {
                _thanhTienCuaMatHangXuatTo = value;
                OnPropertyChanged();
            }
        }

        private DaiLy _selectedDaiLies = new();
        public DaiLy SelectedDaiLies
        {
            get => _selectedDaiLies;
            set
            {
                _selectedDaiLies = value;
                OnPropertyChanged();
            }
        }

        private LoaiDaiLy _selectedLoaiDaiLies = new();
        public LoaiDaiLy SelectedLoaiDaiLies
        {
            get => _selectedLoaiDaiLies;
            set
            {
                _selectedLoaiDaiLies = value;
                OnPropertyChanged();
            }
        }       

        private MatHang _selectedMatHangXuats = new();
        public MatHang SelectedMatHangXuats
        {
            get => _selectedMatHangXuats;
            set
            {
                _selectedMatHangXuats = value;
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

        private DonViTinh _selectedDonViTinhs = new();
        public DonViTinh SelectedDonViTinhs
        {
            get => _selectedDonViTinhs;
            set
            {
                _selectedDonViTinhs = value;
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

        // Search Results
        private ObservableCollection<PhieuXuat> _searchResults = [];
        // Fix for CS1503: Update the type of the `SearchCompleted` event to match the type of `SearchResults`.

        public event EventHandler<ObservableCollection<PhieuXuat>>? SearchCompleted;
        public ObservableCollection<PhieuXuat> SearchResults
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
        public ICommand TraCuuPhieuXuatCommand { get; }

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
                LoaiDaiLies = [.. listLoaiDaiLy];
                DaiLies = [.. listDaiLy];
                Quans = [.. listQuan];
                MatHangXuats = [.. listMatHang];
                DonViTinhs = [.. listDonViTinh];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchPhieuXuat()
        {
            try
            {
                // 1. Kiểm tra nếu chưa nhập gì hết thì hỏi xác nhận
                bool isFilterEmpty =
                    string.IsNullOrEmpty(MaPhieuXuat) &&
                    SelectedDaiLies.MaDaiLy == 0 &&
                    string.IsNullOrEmpty(DienThoai) &&
                    string.IsNullOrEmpty(Email) &&
                    string.IsNullOrEmpty(DiaChi) &&
                    SelectedLoaiDaiLies.MaLoaiDaiLy == 0 &&
                    SelectedQuans.MaQuan == 0 &&
                    NgayTiepNhanFrom == DateTime.MinValue &&
                    NgayTiepNhanTo == DateTime.MinValue &&
                    string.IsNullOrEmpty(NoDaiLyFrom) &&
                    string.IsNullOrEmpty(NoDaiLyTo) &&
                    NgayLapPhieuXuatFrom == DateTime.MinValue &&
                    NgayLapPhieuXuatTo == DateTime.MinValue &&
                    string.IsNullOrEmpty(TongGiaTriPhieuXuatFrom) &&
                    string.IsNullOrEmpty(TongGiaTriPhieuXuatTo) &&
                    SelectedMatHangXuats.MaMatHang == 0 &&
                    SelectedDonViTinhs.MaDonViTinh == 0 &&
                    string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatFrom) &&
                    string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatTo) &&
                    string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatFrom) &&
                    string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatTo) &&
                    string.IsNullOrEmpty(ThanhTienCuaMatHangXuatFrom) &&
                    string.IsNullOrEmpty(ThanhTienCuaMatHangXuatTo) &&
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

                var phieuXuats = await _phieuXuatService.GetAllPhieuXuat();
                ObservableCollection<PhieuXuat> filteredResults = [.. phieuXuats];

                if (!string.IsNullOrEmpty(MaPhieuXuat))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaPhieuXuat.ToString().Contains(MaPhieuXuat))];
                }

                if (SelectedDaiLies.MaDaiLy != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaDaiLy == SelectedDaiLies.MaDaiLy)];
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

                if (SelectedLoaiDaiLies.MaLoaiDaiLy != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.MaLoaiDaiLy == SelectedLoaiDaiLies.MaLoaiDaiLy)];
                }

                if (SelectedQuans.MaQuan != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.MaQuan == SelectedQuans.MaQuan)];
                }

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (NgayTiepNhanFrom != DateTime.MinValue && NgayTiepNhanTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.DaiLy.NgayTiepNhan >= NgayTiepNhanFrom && d.DaiLy.NgayTiepNhan <= NgayTiepNhanTo)];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (NgayTiepNhanFrom != DateTime.MinValue)
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DaiLy.NgayTiepNhan >= NgayTiepNhanFrom)];
                    }
                    // Nếu chỉ nhập To
                    if (NgayTiepNhanTo != DateTime.MinValue)
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DaiLy.NgayTiepNhan <= NgayTiepNhanTo)];
                    }
                }
                //—— hết phần lọc ngày tiếp nhận ——

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(NoDaiLyFrom) && !string.IsNullOrEmpty(NoDaiLyTo)
                    && long.TryParse(NoDaiLyFrom, out var fromNo) && long.TryParse(NoDaiLyTo, out var toNo))
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.DaiLy.TienNo >= fromNo && d.DaiLy.TienNo <= toNo)];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(NoDaiLyFrom) && long.TryParse(NoDaiLyFrom, out fromNo))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DaiLy.TienNo >= fromNo)];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(NoDaiLyTo) && long.TryParse(NoDaiLyTo, out toNo))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DaiLy.TienNo <= toNo)];
                    }
                }
                //—— hết phần lọc nợ đại lý ——

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (NgayLapPhieuXuatFrom != DateTime.MinValue && NgayLapPhieuXuatTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.NgayLapPhieu >= NgayLapPhieuXuatFrom && d.NgayLapPhieu <= NgayLapPhieuXuatTo)];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (NgayLapPhieuXuatFrom != DateTime.MinValue)
                        // Fix for CS1061: Corrected the property name 'PhieuXuat.NgayLapPhieuXuat' to 'PhieuXuat.NgayLapPhieu' 
                        // as per the provided type signature of 'PhieuXuat'.

                        if (NgayLapPhieuXuatFrom != DateTime.MinValue && NgayLapPhieuXuatTo != DateTime.MinValue)
                        {
                            filteredResults = [.. filteredResults
                                .Where(d => d.NgayLapPhieu >= NgayLapPhieuXuatFrom && d.NgayLapPhieu <= NgayLapPhieuXuatTo)];
                        }
                        else
                        {
                            // If only From is provided
                            if (NgayLapPhieuXuatFrom != DateTime.MinValue)
                            {
                                filteredResults = [.. filteredResults
                                    .Where(d => d.NgayLapPhieu >= NgayLapPhieuXuatFrom)];
                            }
                            // If only To is provided
                            if (NgayLapPhieuXuatTo != DateTime.MinValue)
                            {
                                filteredResults = [.. filteredResults
                                    .Where(d => d.NgayLapPhieu <= NgayLapPhieuXuatTo)];
                            }
                        }
                }
                //—— hết phần lọc ngày lập phiếu xuất ——

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(TongGiaTriPhieuXuatFrom) && !string.IsNullOrEmpty(TongGiaTriPhieuXuatTo)
                    && long.TryParse(TongGiaTriPhieuXuatFrom, out var fromTongGiaTri) && long.TryParse(TongGiaTriPhieuXuatTo, out var toTongGiaTri))
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.TongTriGia >= fromTongGiaTri && d.TongTriGia <= toTongGiaTri)];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(TongGiaTriPhieuXuatFrom) && long.TryParse(TongGiaTriPhieuXuatFrom, out fromTongGiaTri))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.TongTriGia >= fromTongGiaTri)];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(TongGiaTriPhieuXuatTo) && long.TryParse(TongGiaTriPhieuXuatTo, out toTongGiaTri))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.TongTriGia <= toTongGiaTri)];
                    }
                }
                //—— hết phần lọc tổng giá trị phiếu xuất ——

                // Fix for CS1061: Replace the incorrect property 'MaMatHangXuat' with the correct property 'MaMatHang' in the condition.

                if (SelectedMatHangXuats.MaMatHang != 0)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MaMatHang == SelectedMatHangXuats.MaMatHang))];
                }

                if (SelectedDonViTinhs.MaDonViTinh != 0)
                {
                    // Fix for CS1061: Removed the incorrect filtering condition for 'MaDonViTinh' as 'PhieuXuat' does not have this property.
                    // Instead, filtering logic should be applied based on related data, such as 'DsChiTietPhieuXuat' or other relevant properties.

                    if (SelectedDonViTinhs.MaDonViTinh != 0)
                    {
                        filteredResults = [.. filteredResults.Where(d =>
                            d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.MaDonViTinh == SelectedDonViTinhs.MaDonViTinh))];
                    }
                }

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatFrom) && !string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatTo)
                    && int.TryParse(SoLuongXuatCuaMatHangXuatFrom, out var fromSoLuong) && int.TryParse(SoLuongXuatCuaMatHangXuatTo, out var toSoLuong))
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat >= fromSoLuong && ct.SoLuongXuat <= toSoLuong))];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatFrom) && int.TryParse(SoLuongXuatCuaMatHangXuatFrom, out fromSoLuong))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat >= fromSoLuong))];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(SoLuongXuatCuaMatHangXuatTo) && int.TryParse(SoLuongXuatCuaMatHangXuatTo, out toSoLuong))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.SoLuongXuat <= toSoLuong))];
                    }
                }                
                //—— hết phần lọc số lượng xuất của mặt hàng xuất ——

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatFrom) && !string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatTo)
                    && long.TryParse(DonGiaXuatCuaMatHangXuatFrom, out var fromDonGia) && long.TryParse(DonGiaXuatCuaMatHangXuatTo, out var toDonGia))
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.DonGia >= fromDonGia && ct.DonGia <= toDonGia))];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatFrom) && long.TryParse(DonGiaXuatCuaMatHangXuatFrom, out fromDonGia))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.DonGia >= fromDonGia))];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(DonGiaXuatCuaMatHangXuatTo) && long.TryParse(DonGiaXuatCuaMatHangXuatTo, out toDonGia))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.DonGia <= toDonGia))];
                    }
                }
                //—— hết phần lọc đơn giá xuất của mặt hàng xuất ——

                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(ThanhTienCuaMatHangXuatFrom) && !string.IsNullOrEmpty(ThanhTienCuaMatHangXuatTo)
                    && int.TryParse(ThanhTienCuaMatHangXuatFrom, out var fromThanhTien) && int.TryParse(ThanhTienCuaMatHangXuatTo, out var toThanhTien))
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien >= fromThanhTien && ct.ThanhTien <= toThanhTien))];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(ThanhTienCuaMatHangXuatFrom) && int.TryParse(ThanhTienCuaMatHangXuatFrom, out fromThanhTien))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien >= fromThanhTien))];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(ThanhTienCuaMatHangXuatTo) && int.TryParse(ThanhTienCuaMatHangXuatTo, out toThanhTien))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.ThanhTien <= toThanhTien))];
                    }
                }
                //—— hết phần lọc thành tiền của mặt hàng xuất ——


                //—— lọc theo khoảng From–To nếu cả hai đều có giá trị hợp lệ ——
                if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom) && !string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo)
                    && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out var fromSoLuongTon) && int.TryParse(SoLuongTonCuaMatHangXuatTo, out var toSoLuongTon))
                {
                    filteredResults = [.. filteredResults
                        .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon >= fromSoLuongTon && ct.MatHang.SoLuongTon <= toSoLuongTon))];
                }
                else
                {
                    // Nếu chỉ nhập From
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatFrom) && int.TryParse(SoLuongTonCuaMatHangXuatFrom, out fromSoLuongTon))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon >= fromSoLuongTon))];
                    }
                    // Nếu chỉ nhập To
                    if (!string.IsNullOrEmpty(SoLuongTonCuaMatHangXuatTo) && int.TryParse(SoLuongTonCuaMatHangXuatTo, out toSoLuongTon))
                    {
                        filteredResults = [.. filteredResults
                            .Where(d => d.DsChiTietPhieuXuat.Any(ct => ct.MatHang.SoLuongTon <= toSoLuongTon))];
                    }
                }
                //—— hết phần lọc số lượng tồn của mặt hàng xuất ——

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

        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
