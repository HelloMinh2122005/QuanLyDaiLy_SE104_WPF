using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;
using QuanLyDaiLy.Views.BaoCaoViews;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public partial class BaoCaoChiTietViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDaiLyService _daiLyService;
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IPhieuThuService _phieuThuService;
        private readonly Func<string, int, BaoCaoDoanhSoViewModel> _monthYearDoanhSoFactory;
        private readonly Func<string, int, BaoCaoCongNoViewModel> _monthYearCongNoFactory;

        public BaoCaoChiTietViewModel(
            IServiceProvider serviceProvider,
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService,
            IPhieuThuService phieuThuService,
            Func<string, int, BaoCaoDoanhSoViewModel> monthYearDoanhSoFactory,
            Func<string, int, BaoCaoCongNoViewModel> monthYearCongNoFactory
            )
        {
            _daiLyService = daiLyService;
            _serviceProvider = serviceProvider;
            _phieuXuatService = phieuXuatService;
            _phieuThuService = phieuThuService;
            _monthYearDoanhSoFactory = monthYearDoanhSoFactory;
            _monthYearCongNoFactory = monthYearCongNoFactory;


            InitializeMonthYearOptions();
            _ = InitializeDoanhSoData();
            _ = InitializeCongNoData();
        }

        [ObservableProperty]
        private SeriesCollection _doanhSoSeries = [];

        [ObservableProperty]
        private SeriesCollection _congNoSeries = [];

        [ObservableProperty]
        private string[] _doanhSoLabels = null!;

        [ObservableProperty]
        private string[] _congNoLabels = null!;

        public Func<double, string> CurrencyFormatter { get; set; } = value => value.ToString("N0") + " đ";

        [ObservableProperty]
        private List<string> _monthOptions = [];

        [ObservableProperty]
        private List<int> _yearOptions = [];

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

        [ObservableProperty]
        private string _selectedDoanhSoMonth = $"Tháng {DateTime.Now.Month}";

        partial void OnSelectedDoanhSoMonthChanged(string value)
        {
            UpdateDoanhSoData();
        }

        [ObservableProperty]
        private int _selectedDoanhSoYear = DateTime.Now.Year;

        partial void OnSelectedDoanhSoYearChanged(int value)
        {
            UpdateDoanhSoData();
        }

        [ObservableProperty]
        private string _selectedCongNoMonth = $"Tháng {DateTime.Now.Month}";

        partial void OnSelectedCongNoMonthChanged(string value)
        {
            UpdateCongNoData();
        }

        [ObservableProperty]
        private int _selectedCongNoYear = DateTime.Now.Year;

        partial void OnSelectedCongNoYearChanged(int value)
        {
            UpdateCongNoData();
        }

        // Event for data changes
        public event EventHandler? DataChanged;

        [RelayCommand]
        private void OpenDoanhSoWindow()
        {
            var viewModel_DoanhSo = _monthYearDoanhSoFactory(SelectedDoanhSoMonth, SelectedDoanhSoYear);
            viewModel_DoanhSo.DataChanged += async (sender, e) => await InitializeDoanhSoData();
            var doanhSoWindow = new BaoCaoDoanhSoWindow(viewModel_DoanhSo);
            doanhSoWindow?.Show();
        }

        [RelayCommand]
        private void OpenCongNoWindow()
        {
            var viewModel_CongNow = _monthYearCongNoFactory(SelectedCongNoMonth, SelectedCongNoYear);
            viewModel_CongNow.DataChanged += async (sender, e) => await InitializeCongNoData();
            var congNoWindow = new BaoCaoCongNoWindow(viewModel_CongNow);
            congNoWindow?.Show();
        }

        public void InitializeMonthYearOptions()
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

        public async Task InitializeDoanhSoData()
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
        }

        public async Task InitializeCongNoData()
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

            var filteredData = daiLyCongNoList
                .OrderByDescending(d => d.CongNo)
                .Take(10)
                .ToArray();

            if (!filteredData.Any())
            {
                CongNoLabels = Array.Empty<string>();
                CongNoSeries = new SeriesCollection();
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
        }

        private async void UpdateDoanhSoData()
        {
            await InitializeDoanhSoData();
        }

        private async void UpdateCongNoData()
        {
            await InitializeCongNoData();
        }
    }
}