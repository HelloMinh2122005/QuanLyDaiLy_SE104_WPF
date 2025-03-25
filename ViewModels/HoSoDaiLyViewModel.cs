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

            this.quanService = quanService;
            this.loaiDaiLyService = loaiDaiLyService;
            this.daiLyService = daiLyService;
            this.thamSoService = thamSoService;

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

        private ObservableCollection<LoaiDaiLy> loaiDaiLies = [];
        public ObservableCollection<LoaiDaiLy> LoaiDaiLies
        {
            get => loaiDaiLies;
            set
            {
                loaiDaiLies = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Quan> quans = [];
        public ObservableCollection<Quan> Quans
        {
            get => quans;
            set
            {
                quans = value;
                OnPropertyChanged();
            }
        }

        // Events
        public event EventHandler? DataChanged;

        // Services 
        private readonly ILoaiDaiLyService loaiDaiLyService;
        private readonly IQuanService quanService;
        private readonly IDaiLyService daiLyService;
        private readonly IThamSoService thamSoService;

        // Commands
        public ICommand CloseWindowCommand { get; }
        public ICommand TiepNhanDaiLyCommand { get; }
        public ICommand DaiLyMoiCommand { get; }

        private async Task LoadDataAsync()
        {
            var listLoaiDaiLy = await loaiDaiLyService.GetAllLoaiDaiLy();
            var listQuan = await quanService.GetAllQuan();

            LoaiDaiLies.Clear();
            Quans.Clear();
            LoaiDaiLies = [.. listLoaiDaiLy];
            Quans = [.. listQuan];
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

            if (SelectedLoaiDaiLy == null || SelectedQuan == null)
            {
                MessageBox.Show("Vui lòng chọn loại đại lý và quận!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int soLuongDaiLyTrongQuan = await quanService.GetSoLuongDaiLyTrongQuan(SelectedQuan.MaQuan);
            var thamSo = await thamSoService.GetThamSo();
            int soLuongDaiLyToiDaTrongQuan = thamSo.SoLuongDaiLyToiDa;

            if (soLuongDaiLyTrongQuan >= soLuongDaiLyToiDaTrongQuan)
            {
                MessageBox.Show("Quận đã đạt số lượng đại lý tối đa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MaDaiLy = (await daiLyService.GenerateAvailableId()).ToString();
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
                await daiLyService.AddDaiLy(daiLy);
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