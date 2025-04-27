using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.PhieuThuViews;

namespace QuanLyDaiLy.ViewModels.PhieuThuViewModels
{
    public class ThemPhieuThuWindowViewModel : INotifyPropertyChanged
    {
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

            // Initialize commands
            CloseCommand = new RelayCommand(CloseWindow);
            LapPhieuThuTienCommand = new RelayCommand(LapPhieuThu);

            // Load initial data
            _ = LoadDataAsync();
        }


        // Commands 
        public ICommand CloseCommand { get; }
        public ICommand LapPhieuThuTienCommand { get; }

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
                QuyDinhTienThuTienNo = thamso.QuyDinhTienThuTienNo;
            }
        }
        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<ThemPhieuThuWindow>().FirstOrDefault()?.Close();
        }

        private void LapPhieuThu()
        {
            _ = LapPhieuThuAsync();
        }

        private async Task LapPhieuThuAsync()
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

        #region Binding Properties
        private string _maPhieuThu = string.Empty;
        public string MaPhieuThu
        {
            get => _maPhieuThu;
            set
            {
                _maPhieuThu = value;
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

        private DaiLy _selectedDaiLy = null!;
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                _selectedDaiLy = value;
                OnPropertyChanged();
                // Cập nhật lại thông tin đại lý khi chọn
                if (_selectedDaiLy != null)
                {
                    SoDienThoai = _selectedDaiLy.DienThoai;
                    Email = _selectedDaiLy.Email;
                    DiaChi = _selectedDaiLy.DiaChi;
                    NoDaiLy = _selectedDaiLy.TienNo;
                }
            }
        }

        private string _soDienThoai = string.Empty;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set
            {
                _soDienThoai = value;
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
        private long _noDaiLy = 0;
        public long NoDaiLy
        {
            get => _noDaiLy;
            set
            {
                _noDaiLy = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayThuTien = DateTime.Now;
        public DateTime NgayThuTien
        {
            get => _ngayThuTien;
            set
            {
                _ngayThuTien = value;
                OnPropertyChanged();
            }
        }

        private long _soTienThu = 0;
        public long SoTienThu
        {
            get => _soTienThu;
            set
            {
                _soTienThu = value;
                OnPropertyChanged();
            }
        }

        private bool _quyDinhTienThuTienNo = true;
        public bool QuyDinhTienThuTienNo
        {
            get => _quyDinhTienThuTienNo;
            set
            {
                _quyDinhTienThuTienNo = value;
                OnPropertyChanged();
            }
        }
        #endregion


        // Event to notify parent view when data changes
        public event EventHandler? DataChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
