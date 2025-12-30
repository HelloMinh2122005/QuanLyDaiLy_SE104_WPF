//using QuanLyDaiLy.Commands;

//using QuanLyDaiLy.Models;
//using QuanLyDaiLy.Services;

//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Windows.Input;
//using System.Windows;

using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Models.dto;
using QuanLyDaiLy.Services;
using System.Linq;

using QuanLyDaiLy.Views.PhieuXuatViews;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public partial class CapNhatPhieuXuatWindowViewModel : ObservableObject, IRecipient<SelectedIdMessage>
    {
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IChiTietPhieuXuatService _phieuXuatChiTietService;
        private readonly IDaiLyService _daiLyService;
        private readonly IMatHangService _matHangService;
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private int _phieuXuatID;

        public CapNhatPhieuXuatWindowViewModel(
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

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(SelectedIdMessage message)
        {
            _phieuXuatID = message.Value;
            // Load data
            _ = LoadDataAsync();
        }

        // Properties for binding
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

        [ObservableProperty]
        private ObservableCollection<MatHang> _danhSachMatHang = [];

        [ObservableProperty]
        private ObservableCollection<MatHang> _danhSachMatHangDaChon = [];

        // Methods
        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<CapNhatPhieuXuatWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
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

                var phieuXuat = await _phieuXuatService.GetPhieuXuatById(_phieuXuatID);
                long oldTongTriGia = phieuXuat.TongTriGia;

                phieuXuat.MaDaiLy = SelectedDaiLy.MaDaiLy;
                phieuXuat.NgayLapPhieu = NgayLap;
                phieuXuat.TongTriGia = TongTien;

                await _phieuXuatService.UpdatePhieuXuat(phieuXuat);

                var existingChiTiet = await _phieuXuatChiTietService.GetChiTietPhieuXuatByPhieuXuatId(_phieuXuatID);

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
                        MaPhieuXuat = _phieuXuatID,
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
        [RelayCommand]
        private void UpdateAvailableLists()
        {
            var selectedIds = DanhSachMatHangPhieuXuat.Select(r => r.SelectedMatHang.MaMatHang).ToHashSet();
            foreach (var row in DanhSachMatHangPhieuXuat)
            {
                var own = row.SelectedMatHang;
                var available = DanhSachMatHang
                    .Where(m => !selectedIds.Contains(m.MaMatHang))
                    .Concat(new[] { own })
                    .ToList();
                row.DanhSachMatHang = new ObservableCollection<MatHang>(available);
            }
        }

        [RelayCommand]
        private void ThemMatHang()
        {
            var available = new List<MatHang>(DanhSachMatHang);
            if (!available.Any())
            {
                MessageBox.Show("Đã nhập đầy đủ mặt hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var newItem = new DisplayMatHangPhieuXuat(available);
            newItem.ThanhTienChanged += (s, e) => CalculateTongTien();

            DanhSachMatHangPhieuXuat.Add(newItem);
            DanhSachMatHang.Remove(newItem.SelectedMatHang);
            DanhSachMatHangDaChon.Add(newItem.SelectedMatHang);

            UpdateAvailableLists();
            CalculateTongTien();
        }

        [RelayCommand]
        private void XoaMatHang()
        {
            if (SelectedMatHangPhieuXuat == null!)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng để xóa", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DanhSachMatHangPhieuXuat.Count > 0)
            {
                DanhSachMatHang.Add(SelectedMatHangPhieuXuat.SelectedMatHang);
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
            try
            {
                MaPhieuXuat = _phieuXuatID.ToString();
                var existingPhieuXuat = await _phieuXuatService.GetPhieuXuatById(_phieuXuatID);
                SelectedDaiLy = existingPhieuXuat.DaiLy;
                NgayLap = existingPhieuXuat.NgayLapPhieu;
                TongTien = existingPhieuXuat.TongTriGia;

                DanhSachMatHang = [.. (await _matHangService.GetAllMatHang())];
                var listDaiLy = await _daiLyService.GetAllDaiLy();
                DaiLies = [.. listDaiLy];


                DanhSachMatHangPhieuXuat.Clear();

                var listChiTietPhieuXuat = await _phieuXuatChiTietService.GetChiTietPhieuXuatByPhieuXuatId(_phieuXuatID);
                foreach (var chiTiet in listChiTietPhieuXuat)
                {
                    DanhSachMatHangPhieuXuat.Add(new DisplayMatHangPhieuXuat(DanhSachMatHang)
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
                MessageBox.Show(ex.Message, "Please god don't go in here");
            }
        }
    }
}
