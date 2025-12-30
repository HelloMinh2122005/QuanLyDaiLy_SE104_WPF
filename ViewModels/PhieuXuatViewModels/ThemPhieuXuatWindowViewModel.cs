using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Models.dto;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.PhieuXuatViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public partial class ThemPhieuXuatWindowViewModel : ObservableObject
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

            _ = LoadDataAsync();
        }

        // Other properties 
        private List<MatHang> _danhSachMatHang = [];
        private List<MatHang> _danhSachMatHangDaChon = [];

        [ObservableProperty]
        private string _maPhieuXuat = string.Empty;

        [ObservableProperty]
        private ObservableCollection<DisplayMatHangPhieuXuat> _danhSachMatHangPhieuXuat = [];

        [ObservableProperty]
        private DisplayMatHangPhieuXuat _selectedMatHangPhieuXuat = null!;

        [ObservableProperty]
        private ObservableCollection<DaiLy> _daiLies = [];

        [ObservableProperty]
        private DaiLy _selectedDaiLy = null!;

        [ObservableProperty]
        private long _noToiDa = 0;

        [ObservableProperty]
        private long _tienNo = 0;

        [ObservableProperty]
        private DateTime _ngayLap = DateTime.Now;

        [ObservableProperty]
        private long _tongTien = 0;


        // Methods for commands
        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ThemPhieuXuatWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
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

        [RelayCommand]
        private void PhieuXuatMoi()
        {
            SelectedDaiLy = null!;
            SelectedMatHangPhieuXuat = null!;
            TongTien = 0;
            _ = LoadDataAsync();
        }

        [RelayCommand]
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

        [RelayCommand]
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

        [RelayCommand]
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

        [RelayCommand]
        private void BoChonMatHang()
        {
            SelectedMatHangPhieuXuat = null!;
        }

        [RelayCommand]
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

        [RelayCommand]
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
    }
}
