using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Views.PhieuThuViews;

namespace QuanLyDaiLy.ViewModels.PhieuThuViewModels
{
    public class TraCuuPhieuThuWindowViewModel : INotifyPropertyChanged
    {
        private IPhieuThuService _phieuThuService;
        private IQuanService _quanService;
        private IDaiLyService _daiLyService;
        private ILoaiDaiLyService _loaiDaiLyService;

        // Command
        public ICommand TraCuuPhieuThuCommand { get; }
        public ICommand CloseCommand { get; }

        // services 

        public TraCuuPhieuThuWindowViewModel(
            IPhieuThuService phieuThuService,
            IQuanService quanService,
            IDaiLyService daiLyService,
            ILoaiDaiLyService loaiDaiLyService
        )
        {
            // Initialize services
            _phieuThuService = phieuThuService;
            _quanService = quanService;
            _daiLyService = daiLyService;
            _loaiDaiLyService = loaiDaiLyService;

            // Initialize commands
            TraCuuPhieuThuCommand = new RelayCommand(async () => await SearchPhieuThu());
            CloseCommand = new RelayCommand(CloseWindow);

            _ = LoadData();
        }

        private async Task SearchPhieuThu()
        {
            try
            {
                var phieuThus = await _phieuThuService.GetAllPhieuThu();
                ObservableCollection<PhieuThu> filteredResults = [.. phieuThus];

                if (!string.IsNullOrEmpty(MaPhieuThu))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaPhieuThu.ToString().Contains(MaPhieuThu))];
                }
                if (!string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.MaDaiLy == SelectedDaiLy.MaDaiLy)];
                }
                if (!string.IsNullOrEmpty(DienThoai))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.DienThoai.Contains(DienThoai))];
                }
                if (!string.IsNullOrEmpty(DiaChi))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.DiaChi.Contains(DiaChi))];
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.Email.Contains(Email))];
                }
                if (!string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.MaLoaiDaiLy == SelectedLoaiDaiLy.MaLoaiDaiLy)];
                }
                if (!string.IsNullOrEmpty(SelectedQuan.TenQuan))
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.MaQuan == SelectedQuan.MaQuan)];
                }
                if (NgayTiepNhanFrom != DateTime.MinValue && NgayTiepNhanTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.NgayTiepNhan >= NgayTiepNhanFrom && d.DaiLy.NgayTiepNhan <= NgayTiepNhanTo)];
                }
                if (NgayThuTienFrom != DateTime.MinValue && NgayThuTienTo != DateTime.MinValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.NgayThuTien >= NgayThuTienFrom && d.NgayThuTien <= NgayThuTienTo)];
                }
                if (NoDaiLyFrom != 0 || NoDaiLyTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.DaiLy.TienNo >= NoDaiLyFrom && d.DaiLy.TienNo <= NoDaiLyTo)];
                }
                if (SoTienThuFrom != 0 || SoTienThuTo != long.MaxValue)
                {
                    filteredResults = [.. filteredResults.Where(d => d.SoTienThu >= SoTienThuFrom && d.SoTienThu <= SoTienThuTo)];
                }

                SearchResults = [.. filteredResults];

                // Raise the event with the search results
                SearchCompleted?.Invoke(this, SearchResults);
                ApplySearchResults();

                if (SearchResults.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy kết quả nào phù hợp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySearchResults()
        {
            // Trigger the event with current search results
            SearchCompleted?.Invoke(this, SearchResults);

            // Close the window after applying
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TraCuuPhieuThuTienWindow>().FirstOrDefault()?.Close();
        }

        private async Task LoadData()
        {
            try
            {
                var listDaiLy = await _daiLyService.GetAllDaiLy();
                var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
                var listQuan = await _quanService.GetAllQuan();

                DaiLies.Clear();
                LoaiDaiLies.Clear();
                Quans.Clear();

                DaiLies = [.. listDaiLy];
                LoaiDaiLies = [.. listLoaiDaiLy];
                Quans = [.. listQuan];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private DaiLy _selectedDaiLy = new();
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                _selectedDaiLy = value;
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

        private string _dienThoai = string.Empty;
        public string DienThoai
        {
            get => _dienThoai;
            set
            {
                _dienThoai = value;
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

        private DateTime _ngayTiepNhanFrom = DateTime.MinValue;
        public DateTime NgayTiepNhanFrom
        {
            get => _ngayTiepNhanFrom;
            set
            {
                _ngayTiepNhanFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayTiepNhanTo = DateTime.Now;
        public DateTime NgayTiepNhanTo
        {
            get => _ngayTiepNhanTo;
            set
            {
                _ngayTiepNhanTo = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayThuTienFrom = DateTime.MinValue;
        public DateTime NgayThuTienFrom
        {
            get => _ngayThuTienFrom;
            set
            {
                _ngayThuTienFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _ngayThuTienTo = DateTime.Now;
        public DateTime NgayThuTienTo
        {
            get => _ngayThuTienTo;
            set
            {
                _ngayThuTienTo = value;
                OnPropertyChanged();
            }
        }

        private long _noDaiLyFrom = 0;
        public long NoDaiLyFrom
        {
            get => _noDaiLyFrom;
            set
            {
                _noDaiLyFrom = value;
                OnPropertyChanged();
            }
        }

        private long _noDaiLyTo = long.MaxValue;
        public long NoDaiLyTo
        {
            get => _noDaiLyTo;
            set
            {
                _noDaiLyTo = value;
                OnPropertyChanged();
            }
        }

        private long _soTienThuFrom = 0;
        public long SoTienThuFrom
        {
            get => _soTienThuFrom;
            set
            {
                _soTienThuFrom = value;
                OnPropertyChanged();
            }
        }

        private long _soTienThuTo = long.MaxValue;
        public long SoTienThuTo
        {
            get => _soTienThuTo;
            set
            {
                _soTienThuTo = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PhieuThu> _searchResults = [];
        public ObservableCollection<PhieuThu> SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<ObservableCollection<PhieuThu>>? SearchCompleted;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
