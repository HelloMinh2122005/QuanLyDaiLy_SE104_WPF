using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models.dto;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.PhieuXuatViews;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public class CapNhatPhieuXuatWindowViewModel : INotifyPropertyChanged
    {
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IChiTietPhieuXuatService _phieuXuatChiTietService;
        private readonly IDaiLyService _daiLyService;
        private readonly IMatHangService _matHangService;
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly int _maPhieuXuatPassed;

        public CapNhatPhieuXuatWindowViewModel(
            IPhieuXuatService phieuXuatService,
            IChiTietPhieuXuatService phieuXuatChiTietService,
            IDaiLyService daiLyService,
            IMatHangService matHangService,
            ILoaiDaiLyService loaiDaiLyService,
            int maPhieuXuatPassed
        )
        {
            _phieuXuatService = phieuXuatService;
            _phieuXuatChiTietService = phieuXuatChiTietService;
            _daiLyService = daiLyService;
            _matHangService = matHangService;
            _loaiDaiLyService = loaiDaiLyService;
            _maPhieuXuatPassed = maPhieuXuatPassed;

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            CapNhatPhieuXuatCommand = new RelayCommand(CapNhatPhieuXuat);
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
        public ICommand CapNhatPhieuXuatCommand { get; }
        public ICommand ThemMatHangCommand { get; }
        public ICommand XoaMatHangCommand { get; }
        public ICommand BoChonMatHangCommand { get; }


        // Methods for commands
        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<CapNhatPhieuXuatWindow>().FirstOrDefault()?.Close();
        }

        private void CapNhatPhieuXuat()
        {
            // Use Task.Run to execute the async method without awaiting
            _ = CapNhatPhieuXuatAsync();
        }

        private async Task CapNhatPhieuXuatAsync()
        {
            try
            {
                if (DanhSachMatHangPhieuXuat.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một mặt hàng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var item in DanhSachMatHangPhieuXuat)
                {
                    if (item.SoLuongXuat <= 0)
                    {
                        MessageBox.Show($"Số lượng xuất của {item.SelectedMatHang.TenMatHang} phải lớn hơn 0",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (item.SoLuongXuat > item.SoLuongTon)
                    {
                        MessageBox.Show($"Số lượng xuất của {item.SelectedMatHang.TenMatHang} không được vượt quá số lượng tồn",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (item.DonGiaXuat <= 0)
                    {
                        MessageBox.Show($"Đơn giá xuất của {item.SelectedMatHang.TenMatHang} phải lớn hơn 0",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                var phieuXuat = await _phieuXuatService.GetPhieuXuatById(_maPhieuXuatPassed);
                long oldTongTriGia = phieuXuat.TongTriGia;

                phieuXuat.MaDaiLy = SelectedDaiLy.MaDaiLy;
                phieuXuat.NgayLapPhieu = NgayLap;
                phieuXuat.TongTriGia = TongTien;

                await _phieuXuatService.UpdatePhieuXuat(phieuXuat);

                var existingChiTiet = await _phieuXuatChiTietService.GetChiTietPhieuXuatByPhieuXuatId(_maPhieuXuatPassed);

                foreach (var chiTiet in existingChiTiet)
                {
                    var matHang = await _matHangService.GetMatHangById(chiTiet.MaMatHang);
                    matHang.SoLuongTon += chiTiet.SoLuongXuat;
                    await _matHangService.UpdateMatHang(matHang);
                    await _phieuXuatChiTietService.DeleteChiTietPhieuXuat(chiTiet.MaChiTietPhieuXuat);
                }

                foreach (var item in DanhSachMatHangPhieuXuat)
                {
                    var chiTietPhieuXuat = new ChiTietPhieuXuat
                    {
                        MaPhieuXuat = _maPhieuXuatPassed,
                        MaMatHang = item.SelectedMatHang.MaMatHang,
                        SoLuongXuat = item.SoLuongXuat,
                        DonGia = item.DonGiaXuat,
                        ThanhTien = item.ThanhTien
                    };

                    await _phieuXuatChiTietService.AddChiTietPhieuXuat(chiTietPhieuXuat);

                    var matHang = await _matHangService.GetMatHangById(item.SelectedMatHang.MaMatHang);
                    matHang.SoLuongTon -= item.SoLuongXuat;
                    await _matHangService.UpdateMatHang(matHang);
                }

                var daiLy = await _daiLyService.GetDaiLyById(SelectedDaiLy.MaDaiLy);
                daiLy.TienNo = daiLy.TienNo - oldTongTriGia + TongTien;

                await _daiLyService.UpdateDaiLy(daiLy);

                MessageBox.Show("Cập nhật phiếu xuất thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi cập nhật phiếu xuất: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Cập nhật danh sách available cho tất cả các dòng
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
        //    if (_danhSachMatHang.Count != 0)
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
        //private void ThemMatHang()
        //{
        //    var available = new List<MatHang>(_danhSachMatHang);
        //    if (available.Count == 0)
        //    {
        //        MessageBox.Show("Đã nhập đầy đủ mặt hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //        return;
        //    }

        //    var newItem = new DisplayMatHangPhieuXuat(available, DanhSachMatHangPhieuXuat);
        //    newItem.ThanhTienChanged += (s, e) => CalculateTongTien();

        //    DanhSachMatHangPhieuXuat.Add(newItem);
        //    _danhSachMatHangDaChon.Add(newItem.SelectedMatHang);
        //    _danhSachMatHang.Remove(newItem.SelectedMatHang);

        //    CalculateTongTien();
        //}

        private void XoaMatHang()
        {
            if (SelectedMatHangPhieuXuat == null!)
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
            try
            {
                MaPhieuXuat = _maPhieuXuatPassed.ToString();
                var existingPhieuXuat = await _phieuXuatService.GetPhieuXuatById(_maPhieuXuatPassed);
                SelectedDaiLy = existingPhieuXuat.DaiLy;
                NgayLap = existingPhieuXuat.NgayLapPhieu;
                TongTien = existingPhieuXuat.TongTriGia;

                _danhSachMatHang = [.. (await _matHangService.GetAllMatHang())];
                var listDaiLy = await _daiLyService.GetAllDaiLy();
                DaiLies = [.. listDaiLy];


                DanhSachMatHangPhieuXuat.Clear();

                var listChiTietPhieuXuat = await _phieuXuatChiTietService.GetChiTietPhieuXuatByPhieuXuatId(_maPhieuXuatPassed);
                foreach (var chiTiet in listChiTietPhieuXuat)
                {
                    DanhSachMatHangPhieuXuat.Add(new DisplayMatHangPhieuXuat(_danhSachMatHang)
                    {
                        SelectedMatHang = await _matHangService.GetMatHangById(chiTiet.MaMatHang),
                        SoLuongXuat = chiTiet.SoLuongXuat,
                        DonGiaXuat = chiTiet.DonGia,
                    });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Chỉ phần LoadDataAsync với sửa dòng Add
        //private async Task LoadDataAsync()
        //{
        //    try
        //    {
        //        MaPhieuXuat = _maPhieuXuatPassed.ToString();
        //        var existingPhieuXuat = await _phieuXuatService.GetPhieuXuatById(_maPhieuXuatPassed);
        //        SelectedDaiLy = existingPhieuXuat.DaiLy;
        //        NgayLap = existingPhieuXuat.NgayLapPhieu;
        //        TongTien = existingPhieuXuat.TongTriGia;

        //        // Lấy danh sách nguồn
        //        _danhSachMatHang = new List<MatHang>(await _matHangService.GetAllMatHang());
        //        var listDaiLy = await _daiLyService.GetAllDaiLy();
        //        DaiLies = new ObservableCollection<DaiLy>(listDaiLy);

        //        DanhSachMatHangPhieuXuat.Clear();
        //        var listChiTietPhieuXuat = await _phieuXuatChiTietService.GetChiTietPhieuXuatByPhieuXuatId(_maPhieuXuatPassed);
        //        foreach (var chiTiet in listChiTietPhieuXuat)
        //        {
        //            // Sử dụng constructor mới để chặn duplicate
        //            var item = new DisplayMatHangPhieuXuat(_danhSachMatHang, DanhSachMatHangPhieuXuat)
        //            {
        //                SelectedMatHang = await _matHangService.GetMatHangById(chiTiet.MaMatHang),
        //                SoLuongXuat = chiTiet.SoLuongXuat,
        //                DonGiaXuat = chiTiet.DonGia
        //            };
        //            item.ThanhTienChanged += (s, e) => CalculateTongTien();

        //            DanhSachMatHangPhieuXuat.Add(item);
        //            // Cập nhật nguồn để loại ra mặt hàng đã load
        //            _danhSachMatHang.Remove(item.SelectedMatHang);
        //        }

        //        CalculateTongTien();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}",
        //            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private async Task UpdateTien()
        {
            try
            {
                NoToiDa = (await _loaiDaiLyService.GetLoaiDaiLyById(SelectedDaiLy.MaLoaiDaiLy)).NoToiDa;
                TienNo = SelectedDaiLy.TienNo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Please god don't go in here");
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
