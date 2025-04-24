using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views.BaoCaoViews;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Models;
using System.Windows;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public class BaoCaoChiTietViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDaiLyService _daiLyService;
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IPhieuThuService _phieuThuService;

        public BaoCaoChiTietViewModel(
            IServiceProvider serviceProvider,
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService,
            IPhieuThuService phieuThuService
            )
        {
            _daiLyService = daiLyService;
            _serviceProvider = serviceProvider;
            _phieuXuatService = phieuXuatService;
            _phieuThuService = phieuThuService;

            DoanhSoCommand = new RelayCommand(OpenDoanhSoWindow);
            CongNoCommand = new RelayCommand(OpenCongNoWindow);

            InitializeMonthYearOptions();
            _ = InitializeDoanhSoData();
            _ = InitializeCongNoData();
        }

        public SeriesCollection DoanhSoSeries { get; set; } = [];
        public SeriesCollection CongNoSeries { get; set; } = [];
        public string[] DoanhSoLabels { get; set; } = null!;
        public string[] CongNoLabels { get; set; } = null!;
        public Func<double, string> CurrencyFormatter { get; set; } = value => value.ToString("N0") + " đ";

        public List<string> MonthOptions { get; set; } = [];
        public List<int> YearOptions { get; set; } = [];

        // Tooltips for the charts
        public DefaultTooltip DoanhSoTooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.OnlySender,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = true,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        public DefaultTooltip CongNoTooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.OnlySender,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = true,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        private string _selectedDoanhSoMonth = $"Tháng {DateTime.Now.Month}";
        public string SelectedDoanhSoMonth
        {
            get => _selectedDoanhSoMonth;
            set
            {
                if (_selectedDoanhSoMonth != value)
                {
                    _selectedDoanhSoMonth = value;
                    OnPropertyChanged();
                    UpdateDoanhSoData(); 
                }
            }
        }

        private int _selectedDoanhSoYear = DateTime.Now.Year;
        public int SelectedDoanhSoYear
        {
            get => _selectedDoanhSoYear;
            set
            {
                if (_selectedDoanhSoYear != value)
                {
                    _selectedDoanhSoYear = value;
                    OnPropertyChanged();
                    UpdateDoanhSoData();
                }
            }
        }

        private string _selectedCongNoMonth = $"Tháng {DateTime.Now.Month}";
        public string SelectedCongNoMonth
        {
            get => _selectedCongNoMonth;
            set
            {
                if (_selectedCongNoMonth != value)
                {
                    _selectedCongNoMonth = value;
                    OnPropertyChanged();
                    UpdateCongNoData(); 
                }
            }
        }

        private int _selectedCongNoYear = DateTime.Now.Year;
        public int SelectedCongNoYear
        {
            get => _selectedCongNoYear;
            set
            {
                if (_selectedCongNoYear != value)
                {
                    _selectedCongNoYear = value;
                    OnPropertyChanged();
                    UpdateCongNoData(); 
                }
            }
        }

        // Commands 
        public ICommand DoanhSoCommand { get; }
        public ICommand CongNoCommand { get; }

        private void OpenDoanhSoWindow()
        {
            var doanhSoWindow = _serviceProvider.GetRequiredService<BaoCaoDoanhSoWindow>();
            doanhSoWindow?.Show();
        }

        private void OpenCongNoWindow()
        {
            var congNoWindow = _serviceProvider.GetRequiredService<BaoCaoCongNoWindow>();
            congNoWindow?.Show();
        }

        private void InitializeMonthYearOptions()
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            MonthOptions =
            [
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            ];

            YearOptions = [];
            for (int i = currentYear - 4; i <= currentYear; i++)
            {
                YearOptions.Add(i);
            }

            SelectedDoanhSoMonth = MonthOptions[currentMonth - 1];
            SelectedDoanhSoYear = currentYear;
            SelectedCongNoMonth = MonthOptions[currentMonth - 1];
            SelectedCongNoYear = currentYear;
        }
        private async Task InitializeDoanhSoData()
        {
            if (!int.TryParse(new string(SelectedDoanhSoMonth?.Where(char.IsDigit).ToArray()), out int selectedMonth))
                return;

            int selectedYear = SelectedDoanhSoYear;

            var allPhieuXuatsTask = _phieuXuatService.GetAllPhieuXuat();
            var allDaiLysTask = _daiLyService.GetAllDaiLy();
            await Task.WhenAll(allPhieuXuatsTask, allDaiLysTask);

            var allPhieuXuats = allPhieuXuatsTask.Result
                .Where(p => p.NgayLapPhieu.Month == selectedMonth && p.NgayLapPhieu.Year == selectedYear)
                .ToList();

            var doanhSoDict = allPhieuXuats
                .GroupBy(p => p.MaDaiLy)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.TongTriGia));

            var daiLyDoanhSoList = allDaiLysTask.Result
                .Select(d => new
                {
                    d.TenDaiLy,
                    TongDoanhSo = doanhSoDict.TryGetValue(d.MaDaiLy, out var value) ? value : 0
                })
                .OrderByDescending(d => d.TongDoanhSo)
                .Take(10)
                .ToList();

            DoanhSoLabels = daiLyDoanhSoList.Select(d => d.TenDaiLy).ToArray();
            DoanhSoSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Doanh số",
                    Values = new ChartValues<double>(daiLyDoanhSoList.Select(d => (double)d.TongDoanhSo)),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                    MaxColumnWidth = 50
                }
            };

            OnPropertyChanged(nameof(DoanhSoSeries));
            OnPropertyChanged(nameof(DoanhSoLabels));
        }




        private async Task InitializeCongNoData()
        {
            var selectedMonth = int.Parse(new string(SelectedCongNoMonth.Where(char.IsDigit).ToArray()));
            var selectedYear = SelectedCongNoYear;

            var daiLyList = (await _daiLyService.GetAllDaiLy())?.ToList();

            if (daiLyList == null || !daiLyList.Any())
            {
                CongNoLabels = Array.Empty<string>();
                CongNoSeries = new SeriesCollection();
                return;
            }

            var tasks = daiLyList.Select(async daiLy =>
            {
                var phieuThuTask = _phieuThuService.GetPhieuThuByDaiLyId(daiLy.MaDaiLy);
                var phieuXuatTask = _phieuXuatService.GetPhieuXuatByDaiLyId(daiLy.MaDaiLy);

                await Task.WhenAll(phieuThuTask, phieuXuatTask);

                var phieuThus = (phieuThuTask.Result ?? Enumerable.Empty<PhieuThu>())
                    .Where(p => p.NgayThuTien.Month == selectedMonth && p.NgayThuTien.Year == selectedYear);

                var phieuXuats = (phieuXuatTask.Result ?? Enumerable.Empty<PhieuXuat>())
                    .Where(p => p.NgayLapPhieu.Month == selectedMonth && p.NgayLapPhieu.Year == selectedYear);

                double tongThu = phieuThus.Sum(p => p.SoTienThu);
                double tongXuat = phieuXuats.Sum(p => p.TongTriGia);
                double congNo = tongXuat - tongThu;

                return (TenDaiLy: daiLy.TenDaiLy, CongNo: congNo);
            });

            var daiLyCongNoList = await Task.WhenAll(tasks);

            // Lọc các đại lý có CongNo > 0
            var filteredData = daiLyCongNoList
                //.Where(d => d.CongNo >= 0)
                .OrderByDescending(d => d.CongNo)
                .Take(10)
                .ToArray();

            // Nếu không có đại lý nào có CongNo > 0, không hiển thị gì
            if (!filteredData.Any())
            {
                CongNoLabels = Array.Empty<string>();
                CongNoSeries = new SeriesCollection();
                OnPropertyChanged(nameof(CongNoSeries));
                OnPropertyChanged(nameof(CongNoLabels));
                return;
            }

            CongNoLabels = filteredData.Select(d => d.TenDaiLy).ToArray();
            var sortedDebts = filteredData.Select(d => d.CongNo).ToArray();

            CongNoSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Công nợ",
                    Values = new ChartValues<double>(sortedDebts),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)),
                    MaxColumnWidth = 50
                }
            };

            OnPropertyChanged(nameof(CongNoSeries));
            OnPropertyChanged(nameof(CongNoLabels));
        }




        private async void UpdateDoanhSoData()
        {
            await InitializeDoanhSoData();
        }

        private async void UpdateCongNoData()
        {
            await InitializeCongNoData();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
