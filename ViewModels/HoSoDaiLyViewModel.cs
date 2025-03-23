using QuanLyDaiLy.Views;
using System.Windows.Input;
using System.Windows;
using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Models;
using System.Collections.ObjectModel;
using QuanLyDaiLy.Services;
using System.Threading.Tasks;

namespace QuanLyDaiLy.ViewModels
{
    public class HoSoDaiLyViewModel : INotifyPropertyChanged
    {
        public HoSoDaiLyViewModel(
            IQuanService quanService,
            ILoaiDaiLyService loaiDaiLyService
        ) {
            CloseWindowCommand = new RelayCommand(CloseWindow);
            TiepNhanDaiLyCommand = new RelayCommand(TiepNhanDaiLy);
            DaiLyMoiCommand = new RelayCommand(DaiLyMoi);

            _selectedLoaiDaiLy = new();
            _selectedQuan = new();

            this.quanService = quanService;
            this.loaiDaiLyService = loaiDaiLyService;

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

        private DateTime _ngayTiepNhan;
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

        private LoaiDaiLy _selectedLoaiDaiLy;
        public LoaiDaiLy SelectedLoaiDaiLy
        {
            get => _selectedLoaiDaiLy;
            set
            {
                _selectedLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        private Quan _selectedQuan;
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

        // Services 
        private readonly ILoaiDaiLyService loaiDaiLyService;
        private readonly IQuanService quanService;

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

            _ngayTiepNhan = DateTime.Now;
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<HoSoDaiLyWinDow>().FirstOrDefault()?.Close();
        }

        private void TiepNhanDaiLy()
        {
            MessageBox.Show("Tiếp nhận đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DaiLyMoi()
        {
            MaDaiLy = string.Empty;
            TenDaiLy = string.Empty;
            SoDienThoai = string.Empty;
            Email = string.Empty;
            NgayTiepNhan = DateTime.Now;
            DiaChi = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}