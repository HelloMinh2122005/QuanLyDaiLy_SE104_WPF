using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Models.dto;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.PhieuXuatViews;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public class ThemPhieuXuatWindowViewModel : INotifyPropertyChanged
    {
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IChiTietPhieuXuatService _phieuXuatChiTietService;
        private readonly IDaiLyService _daiLyService;
        private readonly IMatHangService _matHangService;
        private readonly ILoaiDaiLyService _loaiDaiLyService;

        public ThemPhieuXuatWindowViewModel(
            IPhieuXuatService phieuXuatService,
            IChiTietPhieuXuatService phieuXuatChiTietService,
            IDaiLyService daiLyService,
            IMatHangService matHangService,
            ILoaiDaiLyService loaiDaiLyService
        )
        {
            _phieuXuatService = phieuXuatService;
            _phieuXuatChiTietService = phieuXuatChiTietService;
            _daiLyService = daiLyService;
            _matHangService = matHangService;
            _loaiDaiLyService = loaiDaiLyService;

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            LapPhieuXuatCommand = new RelayCommand(LapPhieuXuat);
            PhieuXuatMoiCommand = new RelayCommand(PhieuXuatMoi);
            ThemMatHangCommand = new RelayCommand(ThemMatHang);
            XoaMatHangCommand = new RelayCommand(XoaMatHang);
            BoChonMatHangCommand = new RelayCommand(BoChonMatHang);

            _ = LoadDataAsync();
        }

        // Other properties 
        private List<MatHang> _danhSachMatHang = [];
        private List<MatHang> _danhSachMatHangDaChon = [];

        // Properties for binding
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

        private ObservableCollection<DisplayMatHangPhieuXuat> _danhSachMatHangPhieuXuat = [];
        public ObservableCollection<DisplayMatHangPhieuXuat> DanhSachMatHangPhieuXuat
        {
            get => _danhSachMatHangPhieuXuat;
            set
            {
                _danhSachMatHangPhieuXuat = value;
                OnPropertyChanged();
            }
        }

        private DisplayMatHangPhieuXuat _selectedMatHangPhieuXuat = null!;
        public DisplayMatHangPhieuXuat SelectedMatHangPhieuXuat
        {
            get => _selectedMatHangPhieuXuat;
            set
            {
                _selectedMatHangPhieuXuat = value;
                OnPropertyChanged();
                if (value != null)
                {
                    TongTien = value.SoLuongXuat * value.DonGiaXuat;
                    CalculateTongTien();
                }
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

        private DaiLy _selectedDaiLy = null!;
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                _selectedDaiLy = value;
                OnPropertyChanged();

                if (value != null)
                {
                    _ = UpdateTien();
                }
            }
        }

        private long _noToiDa = 0;
        public long NoToiDa
        {
            get => _noToiDa;
            set
            {
                _noToiDa = value;
                OnPropertyChanged();
            }
        }

        private long _tienNo = 0;
        public long TienNo
        {
            get => _tienNo;
            set
            {
                _tienNo = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayLap = DateTime.Now;
        public DateTime NgayLap
        {
            get => _ngayLap;
            set
            {
                _ngayLap = value;
                OnPropertyChanged();
            }
        }

        private long _tongTien = 0;
        public long TongTien
        {
            get => _tongTien;
            set
            {
                _tongTien = value;
                OnPropertyChanged();
            }
        }

        // Commands 
        public ICommand CloseCommand { get; }
        public ICommand LapPhieuXuatCommand { get; }
        public ICommand PhieuXuatMoiCommand { get; }
        public ICommand ThemMatHangCommand { get; }
        public ICommand XoaMatHangCommand { get; }
        public ICommand BoChonMatHangCommand { get; }
    

        // Methods for commands
        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<ThemPhieuXuatWindow>().FirstOrDefault()?.Close();
        }

        private void LapPhieuXuat()
        {
            // Use Task.Run to execute the async method without awaiting
            _ = LapPhieuXuatAsync();
        }

        private async Task LapPhieuXuatAsync()
        {
            try
            {
                if (SelectedDaiLy == null)
                {
                    MessageBox.Show("Vui lòng chọn đại lý", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DanhSachMatHangPhieuXuat.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một mặt hàng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate quantities and prices
                foreach (var item in DanhSachMatHangPhieuXuat)
                {
                    if (item.SoLuongXuat <= 0)
                    {
                        MessageBox.Show($"Số lượng xuất cho {item.SelectedMatHang.TenMatHang} phải lớn hơn 0",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (item.SoLuongXuat > item.SoLuongTon)
                    {
                        MessageBox.Show($"Số lượng xuất cho {item.SelectedMatHang.TenMatHang} không được vượt quá số lượng tồn ({item.SoLuongTon})",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (item.DonGiaXuat <= 0)
                    {
                        MessageBox.Show($"Đơn giá xuất cho {item.SelectedMatHang.TenMatHang} phải lớn hơn 0",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                //long newDebt = TienNo + TongTien;
                //if (newDebt > NoToiDa)
                //{
                //    var result = MessageBox.Show(
                //        $"Tổng nợ ({newDebt:N0} VND) sau khi xuất hàng sẽ vượt quá nợ tối đa ({NoToiDa:N0} VND).\nBạn có muốn tiếp tục?",
                //        "Cảnh báo",
                //        MessageBoxButton.YesNo,
                //        MessageBoxImage.Warning
                //    );

                //    if (result == MessageBoxResult.No)
                //        return;
                //}

                if (string.IsNullOrEmpty(MaPhieuXuat))
                {
                    int newId = await _phieuXuatService.GenerateAvailableId();
                    MaPhieuXuat = newId.ToString();
                }

                var phieuXuat = new PhieuXuat
                {
                    MaPhieuXuat = int.Parse(MaPhieuXuat),
                    MaDaiLy = SelectedDaiLy.MaDaiLy,
                    NgayLapPhieu = NgayLap,
                    TongTriGia = TongTien
                };

                await _phieuXuatService.AddPhieuXuat(phieuXuat);

                foreach (var item in DanhSachMatHangPhieuXuat)
                {
                    var chiTiet = new ChiTietPhieuXuat
                    {
                        MaPhieuXuat = phieuXuat.MaPhieuXuat,
                        MaMatHang = item.SelectedMatHang.MaMatHang,
                        SoLuongXuat = item.SoLuongXuat,
                        DonGia = item.DonGiaXuat,
                        ThanhTien = item.ThanhTien
                    };

                    await _phieuXuatChiTietService.AddChiTietPhieuXuat(chiTiet);

                    var matHang = await _matHangService.GetMatHangById(item.SelectedMatHang.MaMatHang);
                    matHang.SoLuongTon -= item.SoLuongXuat;
                    await _matHangService.UpdateMatHang(matHang);
                }

                SelectedDaiLy.TienNo += TongTien;
                await _daiLyService.UpdateDaiLy(SelectedDaiLy);

                MessageBox.Show($"Lập phiếu xuất thành công. Mã phiếu xuất: {MaPhieuXuat}",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lập phiếu xuất: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void PhieuXuatMoi()
        {
            SelectedDaiLy = null!;
            SelectedMatHangPhieuXuat = null!;
            TongTien = 0;
            _ = LoadDataAsync();
        }

        private void UpdateAvailableLists()
        {
            var selectedIds = DanhSachMatHangPhieuXuat.Select(r => r.SelectedMatHang.MaMatHang).ToHashSet();
            foreach (var row in DanhSachMatHangPhieuXuat)
            {
                var own = row.SelectedMatHang;
                var available = _danhSachMatHang
                    .Where(m => !selectedIds.Contains(m.MaMatHang))
                    .Concat(new[] { own })
                    .ToList();
                row.DanhSachMatHang = new ObservableCollection<MatHang>(available);
            }
        }

        //private void ThemMatHang()
        //{
        //    if (_danhSachMatHang.Any())
        //    {
        //        SelectedMatHangPhieuXuat = null!;

        //        var newItem = new DisplayMatHangPhieuXuat(_danhSachMatHang);
        //        newItem.ThanhTienChanged += (s, e) => CalculateTongTien();

        //        DanhSachMatHangPhieuXuat.Add(newItem);
        //        _danhSachMatHangDaChon.Add(_danhSachMatHang.First());
        //        _danhSachMatHang.RemoveAt(0);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Đã nhập đầy đủ mặt hàng");
        //    }
        //}

        private void ThemMatHang()
        {
            var available = new List<MatHang>(_danhSachMatHang);
            if (!available.Any())
            {
                MessageBox.Show("Đã nhập đầy đủ mặt hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var newItem = new DisplayMatHangPhieuXuat(available);
            newItem.ThanhTienChanged += (s, e) => CalculateTongTien();

            DanhSachMatHangPhieuXuat.Add(newItem);
            _danhSachMatHang.Remove(newItem.SelectedMatHang);
            _danhSachMatHangDaChon.Add(newItem.SelectedMatHang);

            UpdateAvailableLists();
            CalculateTongTien();
        }

        private void XoaMatHang()
        {
            if (SelectedMatHangPhieuXuat == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng để xóa", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DanhSachMatHangPhieuXuat.Count > 0)
            {
                _danhSachMatHang.Add(SelectedMatHangPhieuXuat.SelectedMatHang);
                DanhSachMatHangPhieuXuat.Remove(SelectedMatHangPhieuXuat);
                CalculateTongTien();
            }
            else
            {
                MessageBox.Show("Không có mặt hàng nào để xóa", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BoChonMatHang()
        {
            SelectedMatHangPhieuXuat = null!;
        }

        private void CalculateTongTien()
        {
            TongTien = DanhSachMatHangPhieuXuat.Sum(item => item.ThanhTien);
            OnPropertyChanged();
        }

        // Load data 
        private async Task LoadDataAsync()
        {
            _danhSachMatHang = new List<MatHang>(await _matHangService.GetAllMatHang());
            var listDaiLy = await _daiLyService.GetAllDaiLy();
            DaiLies = new ObservableCollection<DaiLy>(listDaiLy);
            if (DaiLies.Count > 0)
            {
                SelectedDaiLy = DaiLies.First();
                NoToiDa = (await _loaiDaiLyService.GetLoaiDaiLyById(SelectedDaiLy.MaLoaiDaiLy)).NoToiDa;
                TienNo = SelectedDaiLy.TienNo;
            }
        }

        private async Task UpdateTien()
        {
            try
            {
                NoToiDa = (await _loaiDaiLyService.GetLoaiDaiLyById(SelectedDaiLy.MaLoaiDaiLy)).NoToiDa;
                TienNo = SelectedDaiLy.TienNo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Please god don't go in here" );
            }
        }

        // Event to notify parent view when data changes
        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
