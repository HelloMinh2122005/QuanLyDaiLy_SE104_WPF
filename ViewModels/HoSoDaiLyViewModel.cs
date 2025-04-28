using QuanLyDaiLy.Views;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Models;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.ViewModels
{
    public class HoSoDaiLyViewModel : INotifyPropertyChanged
    {
        public HoSoDaiLyViewModel(
            IQuanService quanService,
            ILoaiDaiLyService loaiDaiLyService,
            IDaiLyService daiLyService,
            IThamSoService thamSoService
        ) {
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanDaiLyCommand = new RelayCommand(async () => await TiepNhanDaiLy());
            DaiLyMoiCommand = new RelayCommand(DaiLyMoi);

            _quanService = quanService;
            _loaiDaiLyService = loaiDaiLyService;
            _daiLyService = daiLyService;
            _thamSoService = thamSoService;

            _ = LoadDataAsync();
        }

        // Properties for binding
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

        private DateTime _ngayTiepNhan = DateTime.Now;
        public DateTime NgayTiepNhan
        {
            get => _ngayTiepNhan;
            set
            {
                _ngayTiepNhan = value;
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

        // Events
        public event EventHandler? DataChanged;

        // Services 
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IQuanService _quanService;
        private readonly IDaiLyService _daiLyService;
        private readonly IThamSoService _thamSoService;

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand TiepNhanDaiLyCommand { get; }
        public ICommand DaiLyMoiCommand { get; }

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


        private void CloseWindow()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<HoSoDaiLyWinDow>().FirstOrDefault()?.Close();
        }

        private async Task TiepNhanDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenDaiLy))
            {
                MessageBox.Show("Tên đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy) || string.IsNullOrEmpty(SelectedQuan.TenQuan))
            {
                MessageBox.Show("Vui lòng chọn loại đại lý và quận!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var soLuongDaiLyTrongQuan = SelectedQuan.DsDaiLy.Count;
            var thamSo = await _thamSoService.GetThamSo();
            var soLuongDaiLyToiDaTrongQuan = thamSo.SoLuongDaiLyToiDa;

            if (soLuongDaiLyTrongQuan >= soLuongDaiLyToiDaTrongQuan)
            {
                MessageBox.Show("Quận đã đạt số lượng đại lý tối đa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

            try
            {
                await _daiLyService.AddDaiLy(daiLy);
                MessageBox.Show("Tiếp nhận đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lưu đại lý không thành công", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DaiLyMoi()
        {
            MaDaiLy = string.Empty;
            TenDaiLy = string.Empty;
            SoDienThoai = string.Empty;
            Email = string.Empty;
            NgayTiepNhan = DateTime.Now;
            DiaChi = string.Empty;
            SelectedLoaiDaiLy = null!;
            SelectedQuan = null!;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}