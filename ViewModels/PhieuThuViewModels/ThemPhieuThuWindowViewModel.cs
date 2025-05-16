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
    public partial class ThemPhieuThuWindowViewModel : ObservableObject
    {
        // Services
        private readonly IPhieuThuService _phieuThuService;
        private readonly IDaiLyService _daiLyService;
        private readonly IThamSoService _thamSoService;

        public ThemPhieuThuWindowViewModel(
            IPhieuThuService phieuThuService,
            IDaiLyService daiLyService,
            IThamSoService thamSoService
        )
        {
            _phieuThuService = phieuThuService;
            _daiLyService = daiLyService;
            _thamSoService = thamSoService;

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var listDaiLy = await _daiLyService.GetAllDaiLy();
            DaiLies = [.. listDaiLy];
            if (DaiLies.Count() > 0)
            {
                SelectedDaiLy = DaiLies.First();
                SoDienThoai = SelectedDaiLy.DienThoai;
                Email = SelectedDaiLy.Email;
                DiaChi = SelectedDaiLy.DiaChi;
                NoDaiLy = SelectedDaiLy.TienNo;
                var thamso = await _thamSoService.GetThamSo();
                _quyDinhTienThuTienNo = thamso.QuyDinhTienThuTienNo;
                if (_quyDinhTienThuTienNo == true)
                    NoiDung = "Đang áp dụng";
                else
                    NoiDung = "Không áp dụng";
            }
        }

        #region Binding Properties
        [ObservableProperty]
        private string _maPhieuThu = string.Empty;
        [ObservableProperty]
        private ObservableCollection<DaiLy> _daiLies = [];

        private DaiLy _selectedDaiLy = null!;
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                SetProperty(ref _selectedDaiLy, value);
                if (_selectedDaiLy != null)
                {
                    SoDienThoai = _selectedDaiLy.DienThoai;
                    Email = _selectedDaiLy.Email;
                    DiaChi = _selectedDaiLy.DiaChi;
                    NoDaiLy = _selectedDaiLy.TienNo;
                }
            }
        }
        [ObservableProperty]
        private string _soDienThoai = string.Empty;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private string _diaChi = string.Empty;
        [ObservableProperty]
        private long _noDaiLy = 0;
        [ObservableProperty]
        private DateTime _ngayThuTien = DateTime.Now;
        [ObservableProperty]
        private long _soTienThu = 0;
        [ObservableProperty] 
        private string _noiDung = string.Empty;

        private bool _quyDinhTienThuTienNo = true;
        #endregion

        #region RelayCommand
        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ThemPhieuThuWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task LapPhieuThu()
        {
            try
            {
                if (SelectedDaiLy == null)
                {
                    MessageBox.Show("Vui lòng chọn đại lý", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrEmpty(MaPhieuThu))
                {
                    int newId = await _phieuThuService.GenerateAvailableId();
                    MaPhieuThu = newId.ToString();
                }

                var thamso = await _thamSoService.GetThamSo();
                var QuyDinhTienThuTienNo = thamso.QuyDinhTienThuTienNo;
                if (QuyDinhTienThuTienNo && SoTienThu > NoDaiLy)
                {
                    MessageBox.Show("Số tiền thu không được lớn hơn số tiền nợ của đại lý", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var phieuThu = new PhieuThu
                {
                    MaPhieuThu = int.Parse(MaPhieuThu),
                    MaDaiLy = SelectedDaiLy.MaDaiLy,
                    NgayThuTien = NgayThuTien,
                    SoTienThu = SoTienThu
                };

                await _phieuThuService.AddPhieuThu(phieuThu);
                await _daiLyService.UpdateDaiLy(SelectedDaiLy);

                MessageBox.Show($"Lập phiếu xuất thành công. Mã phiếu thu: {MaPhieuThu}",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lập phiếu thu: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    #endregion
}
