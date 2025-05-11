using QuanLyDaiLy.Views;
using System.Windows;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Services;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;

namespace QuanLyDaiLy.ViewModels
{
    public partial class HoSoDaiLyViewModel : ObservableObject
    {
        public HoSoDaiLyViewModel(
            IQuanService quanService,
            ILoaiDaiLyService loaiDaiLyService,
            IDaiLyService daiLyService,
            IThamSoService thamSoService
        )
        {
            _quanService = quanService;
            _loaiDaiLyService = loaiDaiLyService;
            _daiLyService = daiLyService;
            _thamSoService = thamSoService;

            _ = LoadDataAsync();
        }

        // Services 
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IQuanService _quanService;
        private readonly IDaiLyService _daiLyService;
        private readonly IThamSoService _thamSoService;

        #region Binding Properties
        [ObservableProperty]
        private string _maDaiLy = string.Empty;
        [ObservableProperty]
        private string _tenDaiLy = string.Empty;
        [ObservableProperty]
        private string _soDienThoai = string.Empty;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private DateTime _ngayTiepNhan = DateTime.Now;
        [ObservableProperty]
        private string _diaChi = string.Empty;
        [ObservableProperty]
        private LoaiDaiLy _selectedLoaiDaiLy = null!;
        [ObservableProperty]
        private Quan _selectedQuan = null!;
        [ObservableProperty]
        private ObservableCollection<LoaiDaiLy> _loaiDaiLies = [];
        [ObservableProperty]
        private ObservableCollection<Quan> _quans = [];
        #endregion

        private async Task LoadDataAsync()
        {
            var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
            var listQuan = await _quanService.GetAllQuan();

            LoaiDaiLies.Clear();
            Quans.Clear();
            LoaiDaiLies = [.. listLoaiDaiLy];
            Quans = [.. listQuan];

            // Auto-select the first items in both ComboBoxes if available
            if (LoaiDaiLies.Count > 0)
            {
                SelectedLoaiDaiLy = LoaiDaiLies[0];
            }

            if (Quans.Count > 0)
            {
                SelectedQuan = Quans[0];
            }
        }

        #region RelayCommand
        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<HoSoDaiLyWinDow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task TiepNhanDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenDaiLy))
            {
                MessageBox.Show("Tên đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Check SDT
            if (string.IsNullOrWhiteSpace(SoDienThoai))
            {
                MessageBox.Show("Số điện thoại không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!SoDienThoai.All(char.IsDigit) || !(SoDienThoai.Length == 10 || SoDienThoai.Length == 11) || !SoDienThoai.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Check email
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailPattern);
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (char.IsDigit(Email[0]))
            {
                MessageBox.Show("Email không được bắt đầu bằng số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!regex.IsMatch(Email))
            {
                MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int atIndex = Email.IndexOf('@');
            string localPart = Email.Substring(0, atIndex);
            if (localPart.Contains("."))
            {
                MessageBox.Show("Email không được có dấu chấm trong phần trước '@'.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(DiaChi))
            {
                MessageBox.Show("Địa chỉ không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy) || string.IsNullOrEmpty(SelectedQuan.TenQuan))
            {
                MessageBox.Show("Vui lòng chọn loại đại lý và quận!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var thamSo = await _thamSoService.GetThamSo();
            var quyDinhSoLuongDaiLyToiDa = thamSo.QuyDinhSoLuongDaiLyToiDa;
            if (quyDinhSoLuongDaiLyToiDa == true)
            {
                var soLuongDaiLyToiDaTrongQuan = thamSo.SoLuongDaiLyToiDa;
                var soLuongDaiLyTrongQuan = (await _quanService.GetQuanById(SelectedQuan.MaQuan)).DsDaiLy.Count;
                if (soLuongDaiLyTrongQuan >= soLuongDaiLyToiDaTrongQuan)
                {
                    MessageBox.Show("Quận đã đạt số lượng đại lý tối đa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            try
            {
                MaDaiLy = (await _daiLyService.GenerateAvailableId()).ToString();
                DaiLy daiLy = new()
                {
                    MaDaiLy = int.Parse(MaDaiLy),
                    TenDaiLy = TenDaiLy,
                    DienThoai = SoDienThoai,
                    Email = Email,
                    NgayTiepNhan = NgayTiepNhan,
                    DiaChi = DiaChi,
                    MaLoaiDaiLy = SelectedLoaiDaiLy.MaLoaiDaiLy,
                    MaQuan = SelectedQuan.MaQuan,
                    LoaiDaiLy = SelectedLoaiDaiLy,
                    Quan = SelectedQuan
                };

                await _daiLyService.AddDaiLy(daiLy);
                MessageBox.Show("Tiếp nhận đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lưu đại lý không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void DaiLyMoi()
        {
            MaDaiLy = string.Empty;
            TenDaiLy = string.Empty;
            SoDienThoai = string.Empty;
            Email = string.Empty;
            NgayTiepNhan = DateTime.Now;
            DiaChi = string.Empty;
            if (LoaiDaiLies.Count > 0)
                SelectedLoaiDaiLy = LoaiDaiLies[0];
            if (Quans.Count > 0)
                SelectedQuan = Quans[0];
        }
        #endregion
    }
}