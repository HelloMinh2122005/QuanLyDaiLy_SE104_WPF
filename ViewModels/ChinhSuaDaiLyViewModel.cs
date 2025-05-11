using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace QuanLyDaiLy.ViewModels
{
    public partial class ChinhSuaDaiLyViewModel :
        ObservableObject,
        IRecipient<SelectedIdMessage>
    {
        // Services
        private readonly IDaiLyService _daiLyService;
        private readonly IQuanService _quanService;
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private int _daiLyId;
        private readonly IThamSoService _thamSoService;

        public ChinhSuaDaiLyViewModel(
            IDaiLyService daiLyService,
            IQuanService quanService,
            ILoaiDaiLyService loaiDaiLyService,
            IThamSoService thamSoService
        )
        {
            _daiLyService = daiLyService;
            _quanService = quanService;
            _loaiDaiLyService = loaiDaiLyService;
            _thamSoService = thamSoService;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(SelectedIdMessage message)
        {
            _daiLyId = message.Value;
            // Load data
            _ = LoadDataAsync();
        }

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
        private DateTime _ngayTiepNhan;
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

            LoaiDaiLies = [.. listLoaiDaiLy];
            Quans = [.. listQuan];

            try
            {
                var daiLy = await _daiLyService.GetDaiLyById(_daiLyId);
                MaDaiLy = daiLy.MaDaiLy.ToString();
                TenDaiLy = daiLy.TenDaiLy;
                SoDienThoai = daiLy.DienThoai;
                Email = daiLy.Email;
                DiaChi = daiLy.DiaChi;
                NgayTiepNhan = daiLy.NgayTiepNhan;

                // Set selected values
                SelectedLoaiDaiLy = LoaiDaiLies.FirstOrDefault(l => l.MaLoaiDaiLy == daiLy.MaLoaiDaiLy) ?? LoaiDaiLies.FirstOrDefault()!;
                SelectedQuan = Quans.FirstOrDefault(q => q.MaQuan == daiLy.MaQuan) ?? Quans.FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        #region RelayCommand
        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<ChinhSuaDaiLyWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task CapNhatDaiLy()
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

            try
            {
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

                var existingDaiLy = await _daiLyService.GetDaiLyById(_daiLyId);
                existingDaiLy.TenDaiLy = TenDaiLy;
                existingDaiLy.DienThoai = SoDienThoai;
                existingDaiLy.Email = Email;
                existingDaiLy.DiaChi = DiaChi;
                existingDaiLy.NgayTiepNhan = NgayTiepNhan;
                existingDaiLy.MaLoaiDaiLy = SelectedLoaiDaiLy.MaLoaiDaiLy;
                existingDaiLy.MaQuan = SelectedQuan.MaQuan;
                existingDaiLy.LoaiDaiLy = SelectedLoaiDaiLy;
                existingDaiLy.Quan = SelectedQuan;

                await _daiLyService.UpdateDaiLy(existingDaiLy);
                MessageBox.Show("Cập nhật đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    #endregion
}