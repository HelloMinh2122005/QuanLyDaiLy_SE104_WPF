using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using QuanLyDaiLy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuanLyDaiLy.ViewModels.DashboardViewModels
{
    public partial class DashboardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private SeriesCollection _spendingSeries = new SeriesCollection();

        [ObservableProperty]
        private SeriesCollection _daiLyDistributionSeries = new SeriesCollection();

        [ObservableProperty]
        private string[] _monthLabels = new string[12];

        [ObservableProperty]
        private Func<double, string> _currencyFormatter = value => value.ToString("N0") + " đ";

        [ObservableProperty]
        private SeriesCollection _topDaiLySeries = new SeriesCollection();

        [ObservableProperty]
        private string[] _topDaiLyLabels = new string[0];

        [ObservableProperty]
        private SeriesCollection _topDebtDaiLySeries = new SeriesCollection();

        [ObservableProperty]
        private SeriesCollection _doanhSoSeries = new SeriesCollection();

        [ObservableProperty]
        private SeriesCollection _quanDaiLySeries = new SeriesCollection();

        [ObservableProperty]
        private string[] _topDebtDaiLyLabels = new string[0];

        [ObservableProperty]
        private List<string> _monthOptions = new List<string>();

        [ObservableProperty]
        private List<int> _yearOptions = new List<int>();


        // service
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IDaiLyService _daiLyService;
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly IPhieuThuService _phieuThuService;

        // Define a custom tooltip for the line chart
        public DefaultTooltip Tooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.SharedXValues,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = true,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        // Define a separate tooltip for the pie chart
        public DefaultTooltip PieTooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.OnlySender,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = false,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        // Define a dedicated tooltip for the column chart
        public DefaultTooltip ColumnTooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.OnlySender,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = true,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        public DefaultTooltip DebtColumnTooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.OnlySender,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = true,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        public DefaultTooltip DoanhSoToolTip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.SharedXValues,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = true,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        public DefaultTooltip QuanDaiLyTooltip { get; set; } = new DefaultTooltip
        {
            SelectionMode = TooltipSelectionMode.OnlySender,
            FontSize = 16,
            FontFamily = new FontFamily("Nunito"),
            ShowTitle = false,
            Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200))
        };

        public DashboardPageViewModel(
            ILoaiDaiLyService loaiDaiLyService,
            IDaiLyService daiLyService,
            IPhieuXuatService phieuXuatService,
            IPhieuThuService phieuThuService)
        {
            _daiLyService = daiLyService;
            _loaiDaiLyService = loaiDaiLyService;
            _phieuXuatService = phieuXuatService;
            _phieuThuService = phieuThuService;

            // Khởi tạo các dữ liệu cần thiết với async/await
            _ = InitializeMonthYearOptions();

            // Dùng async/await để đảm bảo các tác vụ bất đồng bộ hoàn thành trước khi tiếp tục
            _ = InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            var tasks = new List<Task>
            {
                InitializeLineChart(),
                InitializeDoanhSoChart(),
                InitializePieChart(),
                InitializeColumnChart(),
                InitializeDebtColumnChart(),
                UpdateWidgetDoanhThuTrongNgay(),
                UpdateWidgetDoanhThuThang(),
                UpdateWidgetDoanhThuTrongNam(),
                UpdateWidgetDoanhSoTrongNgay(),
                UpdateWidgetDoanhSoTrongThang(),
                UpdateWidgetDoanhSoTrongNam(),
                UpdateWidgetSoLuongDaiLyDangNo(),
                InitializeQuanPieChart()
            };

            // Đợi tất cả các tác vụ hoàn thành đồng thời
            await Task.WhenAll(tasks);
            await UpdateWidgetTongCongNoDauThang();
            await UpdateWidgetTongNoCuoiThang();

        }

        // Hàm chuyển "Tháng 4" → 4
        private int GetSelectedMonthNumber(string monthText)
        {
            return int.TryParse(monthText.Replace("Tháng ", ""), out int month) ? month : 1;
        }

        private Brush RandomColorBrush(int key)
        {
            var colors = new[]
            {
                Color.FromRgb(33, 150, 243),
                Color.FromRgb(76, 175, 80),
                Color.FromRgb(255, 152, 0),
                Color.FromRgb(233, 30, 99),
                Color.FromRgb(156, 39, 176),
                Color.FromRgb(0, 188, 212)
            };

            var color = colors[key % colors.Length];
            return new SolidColorBrush(color);
        }


        #region hàm khởi tạo

        private async Task InitializeLineChart()
        {
            // 1. Lấy tháng/năm hiện tại và năm trước
            int currentYear = _revenueChartYear;
            int lastYear = currentYear - 1;

            // 2. Khởi tạo labels & formatter
            MonthLabels = new[]
            {
                "Tháng 1","Tháng 2","Tháng 3","Tháng 4",
                "Tháng 5","Tháng 6","Tháng 7","Tháng 8",
                "Tháng 9","Tháng 10","Tháng 11","Tháng 12"
            };
            CurrencyFormatter = value => value.ToString("N0") + " đ";

            // 3. Lấy dữ liệu
            var allPhieuThu = await _phieuThuService
                .GetPhieuThuByCurrentYearAndLastYear(currentYear, lastYear);

            // 4. Tính tổng theo tháng
            var thisYearData = new double[12];
            var lastYearData = new double[12];
            foreach (var p in allPhieuThu)
            {
                int m = p.NgayThuTien.Month;
                if (p.NgayThuTien.Year == currentYear) thisYearData[m - 1] += p.SoTienThu;
                else if (p.NgayThuTien.Year == lastYear) lastYearData[m - 1] += p.SoTienThu;
            }

            // 5. Tính bước trục Y: chia đều thành 5 đoạn
            double maxThis = thisYearData.Any() ? thisYearData.Max() : 0;
            double maxLast = lastYearData.Any() ? lastYearData.Max() : 0;
            double maxAll = Math.Max(maxThis, maxLast);
            double rawStep = maxAll / 5.0;
            if (rawStep <= 0)
            {
                // không có dữ liệu ⇒ mặc định 1 triệu
                YAxisStepLineChart = 1_000_000;
            }
            else
            {
                // xác định bậc lớn nhất, ví dụ rawStep=4.68e6 ⇒ magnitude=1e6
                double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
                // làm tròn lên thành niceStep = ceil(rawStep/magnitude)*magnitude
                double niceStep = Math.Ceiling(rawStep / magnitude) * magnitude;
                YAxisStepLineChart = niceStep;
            }
            OnPropertyChanged(nameof(YAxisStepLineChart));

            // 6. Khởi tạo series
            SpendingSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title            = currentYear.ToString(),
                    Values           = new ChartValues<double>(thisYearData),
                    PointGeometry    = DefaultGeometries.Circle,
                    PointGeometrySize= 12,
                    LineSmoothness   = 0,
                    Stroke           = Brushes.Gray,
                    Fill             = Brushes.Transparent,
                    DataLabels       = false,
                    LabelPoint       = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness  = 3
                },
                new LineSeries
                {
                    Title            = lastYear.ToString(),
                    Values           = new ChartValues<double>(lastYearData),
                    PointGeometry    = DefaultGeometries.Square,
                    PointGeometrySize= 12,
                    LineSmoothness   = 1,
                    Stroke           = Brushes.DodgerBlue,
                    Fill             = Brushes.Transparent,
                    DataLabels       = false,
                    LabelPoint       = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness  = 2
                }
            };
            OnPropertyChanged(nameof(SpendingSeries));
        }

        private async Task InitializePieChart()
        {
            // Lấy tháng/năm đang chọn
            int selectedMonth = GetSelectedMonthNumber(_pieChartMonth); // ví dụ từ "Tháng 4" => 4
            int selectedYear = _pieChartYear;
            var endDate = new DateTime(selectedYear, selectedMonth,
                DateTime.DaysInMonth(selectedYear, selectedMonth));


            // 2. Lấy toàn bộ loại đại lý
            var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();

            // 3. Lấy toàn bộ đại lý từ khi tạo app
            var allDaiLy = await _daiLyService.GetAllDaiLy();

            // 4. Lọc đại lý có NgayTiepNhan <= endDate và nhóm theo loại
            var daiLyCounts = allDaiLy
                .Where(d => d.NgayTiepNhan <= endDate)
                .GroupBy(d => d.MaLoaiDaiLy)
                .ToDictionary(g => g.Key, g => g.Count());

            // 5. Xây SeriesCollection cho PieChart
            DaiLyDistributionSeries = new SeriesCollection();
            foreach (var loai in listLoaiDaiLy)
            {
                daiLyCounts.TryGetValue(loai.MaLoaiDaiLy, out int count);

                DaiLyDistributionSeries.Add(new PieSeries
                {
                    Title = $"{loai.TenLoaiDaiLy} ({count})",
                    Values = new ChartValues<double> { count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = RandomColorBrush(loai.MaLoaiDaiLy)
                });
            }

            // 6. Notify UI
            OnPropertyChanged(nameof(DaiLyDistributionSeries));

        }

        private async Task InitializeColumnChart()
        {
            // 1. Lấy tháng/năm đang chọn
            int selectedMonth = GetSelectedMonthNumber(_topAgentChartMonth);
            int selectedYear = _topAgentChartYear;

            // 2. Lấy tổng TongTriGia của mỗi đại lý
            var totalDic = await _phieuXuatService
                .GetTotalValueByDaiLyAsync(selectedMonth, selectedYear);

            // 3. Sắp xếp giảm dần và chỉ lấy 5 đại lý đầu tiên
            var top5Dic = totalDic
                .OrderByDescending(kv => kv.Value)
                .Take(5)
                .ToList();

            // 4. Lấy thông tin DaiLy cho top 5
            var topIds = top5Dic.Select(kv => kv.Key).ToList();
            var daiLyList = await _daiLyService.GetDaiLysByIdsAsync(topIds);

            // 5. Chuẩn bị mảng tên và giá trị (giữ đúng thứ tự top5Dic)
            var dailyNames = new List<string>();
            var dailyValues = new List<double>();
            foreach (var kv in top5Dic)
            {
                var d = daiLyList.FirstOrDefault(x => x.MaDaiLy == kv.Key);
                if (d != null)
                {
                    dailyNames.Add(d.TenDaiLy);
                    dailyValues.Add(kv.Value);
                }
            }

            // 6. Tính bước trục Y (YAxisStepColumnChart) chia đều 5 đoạn và làm tròn “đẹp”
            double maxValue = dailyValues.Any() ? dailyValues.Max() : 0;
            double rawStep = maxValue / 5.0;
            if (rawStep <= 0)
            {
                // Không có dữ liệu hoặc tất cả bằng 0 → dùng mặc định 1 triệu
                YAxisStepColumnChart = 1_000_000;
            }
            else
            {
                double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
                YAxisStepColumnChart = Math.Ceiling(rawStep / magnitude) * magnitude;
            }
            OnPropertyChanged(nameof(YAxisStepColumnChart));

            // 6. Cập nhật nhãn X và notify
            TopDaiLyLabels = dailyNames.ToArray();
            OnPropertyChanged(nameof(TopDaiLyLabels));

            // 7. Khởi tạo lại SeriesCollection cho chart
            TopDaiLySeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title          = "Tổng giá trị",
                    Values         = new ChartValues<double>(dailyValues),
                    DataLabels     = true,
                    LabelPoint     = p => p.Y.ToString("N0") + " đ",
                    Fill           = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                    MaxColumnWidth = 70
                }
            };
            OnPropertyChanged(nameof(TopDaiLySeries));
        }

        private async Task InitializeDebtColumnChart()
        {
            // 1. Lấy tháng/năm đang chọn
            int selectedMonth = GetSelectedMonthNumber(_debtChartMonth);
            int selectedYear = _debtChartYear;

            DateTime startDate = new DateTime(selectedYear, selectedMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // 2. Lấy danh sách phiếu xuất và phiếu thu trong khoảng thời gian đó
            var phieuXuats = await _phieuXuatService.GetPhieuXuatByDateRange(startDate, endDate);
            var phieuThus = await _phieuThuService.GetPhieuThuByDateRange(startDate, endDate);


            // 3. Tính tổng phiếu xuất theo đại lý
            var tongXuatTheoDaiLy = phieuXuats
                .GroupBy(p => p.DaiLy)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.TongTriGia)
                );

            // 4. Tính tổng phiếu thu theo đại lý
            var tongThuTheoDaiLy = phieuThus
                .GroupBy(p => p.DaiLy)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.SoTienThu)
                );



            // 5. Lấy toàn bộ đại lý
            var allDaiLys = await _daiLyService.GetAllDaiLy();

            // 6. Tính công nợ
            var congNoTheoDaiLy = allDaiLys
                .Select(daiLy =>
                {
                    double tongXuat = tongXuatTheoDaiLy.ContainsKey(daiLy) ? tongXuatTheoDaiLy[daiLy] : 0;
                    double tongThu = tongThuTheoDaiLy.ContainsKey(daiLy) ? tongThuTheoDaiLy[daiLy] : 0;
                    return new { daiLy.TenDaiLy, TienNo = tongXuat - tongThu };
                })
                .Where(d => d.TienNo >= 0)
                .OrderByDescending(d => d.TienNo)
                .Take(10)
                .ToList();

            // 7. Cập nhật nhãn và giá trị cho biểu đồ
            var debtNames = congNoTheoDaiLy.Select(d => d.TenDaiLy).ToList();
            var debtValues = congNoTheoDaiLy.Select(d => d.TienNo).ToList();

            TopDebtDaiLyLabels = debtNames.ToArray();
            OnPropertyChanged(nameof(TopDebtDaiLyLabels));

            // 8. Tính bước trục Y (YAxisStepDebtColumnChart)
            double maxValue = debtValues.Any() ? debtValues.Max() : 0;
            double rawStep = maxValue / 5.0;
            if (rawStep <= 0)
            {
                YAxisStepDebtColumnChart = 1_000_000;
            }
            else
            {
                double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
                YAxisStepDebtColumnChart = Math.Ceiling(rawStep / magnitude) * magnitude;
            }
            OnPropertyChanged(nameof(YAxisStepDebtColumnChart));

            TopDebtDaiLySeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Công nợ",
                    Values = new ChartValues<double>(debtValues),
                    DataLabels = true,
                    LabelPoint = p => p.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)),
                    MaxColumnWidth = 50
                }
            };

            OnPropertyChanged(nameof(TopDebtDaiLySeries));
        }

        private async Task InitializeMonthYearOptions()
        {
            // Get current date info
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            // 1. Lấy ngày đầu tiên có đại lý
            var earliest = await _daiLyService.GetEarliestDaiLyDateAsync();
            int startYear = earliest?.Year ?? currentYear;

            // Setup month options (Vietnamese)
            MonthOptions =
            [
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            ];

            // Setup year options (current year and 4 previous years)
            YearOptions = [];
            for (int y = startYear; y <= currentYear; y++)
            {
                YearOptions.Add(y);
            }

            // Set default selections for all combo boxes
            _revenueChartMonth = MonthOptions[currentMonth - 1];
            _revenueChartYear = currentYear;
            _pieChartMonth = MonthOptions[currentMonth - 1];
            _pieChartYear = currentYear;
            _topAgentChartMonth = MonthOptions[currentMonth - 1];
            _topAgentChartYear = currentYear;
            _debtChartMonth = MonthOptions[currentMonth - 1];
            _debtChartYear = currentYear;
            _DoanhSoChartMonth = MonthOptions[currentMonth - 1];
            _DoanhSoChartYear = currentYear;
        }

        private async Task InitializeDoanhSoChart()
        {
            // 1. Lấy tháng/năm hiện tại và năm trước
            int currentYear = _DoanhSoChartYear;
            int lastYear = currentYear - 1;

            // 2. Khởi tạo labels & formatter
            MonthLabels = new[]
            {
                "Tháng 1","Tháng 2","Tháng 3","Tháng 4",
                "Tháng 5","Tháng 6","Tháng 7","Tháng 8",
                "Tháng 9","Tháng 10","Tháng 11","Tháng 12"
            };
            CurrencyFormatter = value => value.ToString("N0") + " đ";

            // 3. Lấy dữ liệu doanh số
            var allDoanhSo = await _phieuXuatService.GetPhieuXuatByCurrentYearAndLastYear(currentYear, lastYear);

            // 4. Tính tổng doanh số theo tháng
            var thisYearData = new double[12];
            var lastYearData = new double[12];
            foreach (var p in allDoanhSo)
            {
                int m = p.NgayLapPhieu.Month;
                if (p.NgayLapPhieu.Year == currentYear) thisYearData[m - 1] += p.TongTriGia;
                else if (p.NgayLapPhieu.Year == lastYear) lastYearData[m - 1] += p.TongTriGia;
            }

            // 5. Tính bước trục Y: chia đều thành 5 đoạn
            double maxThis = thisYearData.Any() ? thisYearData.Max() : 0;
            double maxLast = lastYearData.Any() ? lastYearData.Max() : 0;
            double maxAll = Math.Max(maxThis, maxLast);
            double rawStep = maxAll / 5.0;
            if (rawStep <= 0)
            {
                // không có dữ liệu ⇒ mặc định 1 triệu
                YAxisStepDoanhSoChart = 1_000_000;
            }
            else
            {
                // xác định bậc lớn nhất, ví dụ rawStep=4.68e6 ⇒ magnitude=1e6
                double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
                // làm tròn lên thành niceStep = ceil(rawStep/magnitude)*magnitude
                double niceStep = Math.Ceiling(rawStep / magnitude) * magnitude;
                YAxisStepDoanhSoChart = niceStep;
            }

            // 6. Khởi tạo series
            DoanhSoSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title            = currentYear.ToString(),
                    Values           = new ChartValues<double>(thisYearData),
                    PointGeometry    = DefaultGeometries.Diamond,
                    PointGeometrySize= 12,
                    LineSmoothness   = 0,
                    Stroke = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                    Fill             = Brushes.Transparent,
                    DataLabels       = false,
                    LabelPoint       = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness  = 2
                },
                new LineSeries
                {
                    Title            = lastYear.ToString(),
                    Values           = new ChartValues<double>(lastYearData),
                    PointGeometry    = DefaultGeometries.Triangle,
                    PointGeometrySize= 12,
                    LineSmoothness   = 1,
                    Stroke = new SolidColorBrush(Color.FromRgb(255, 152, 0)),
                    Fill             = Brushes.Transparent,
                    DataLabels       = false,
                    LabelPoint       = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness  = 3
                }
            };

        }


        private async Task InitializeQuanPieChart()
        {
            // Lấy tháng/năm đang chọn (reuse the same selection as the LoaiDaiLy pie chart)
            int selectedMonth = GetSelectedMonthNumber(_pieChartMonth);
            int selectedYear = _pieChartYear;
            var endDate = new DateTime(selectedYear, selectedMonth,
                DateTime.DaysInMonth(selectedYear, selectedMonth));

            // Lấy toàn bộ đại lý từ khi tạo app
            var allDaiLy = await _daiLyService.GetAllDaiLy();

            // Lọc đại lý có NgayTiepNhan <= endDate và nhóm theo quận
            var daiLyCountsByQuan = allDaiLy
                .Where(d => d.NgayTiepNhan <= endDate)
                .GroupBy(d => d.MaQuan)
                .Select(g => new { MaQuan = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            // Xây SeriesCollection cho PieChart
            QuanDaiLySeries = new SeriesCollection();

            // Populate with data from each district
            foreach (var group in daiLyCountsByQuan)
            {
                var quan = group.MaQuan; // Ideally we'd get the name but we'll just use ID for now
                QuanDaiLySeries.Add(new PieSeries
                {
                    Title = $"Quận {quan} ({group.Count})",
                    Values = new ChartValues<double> { group.Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = RandomColorBrush(quan) // Reuse the same random color function
                });
            }

            // Notify UI
            OnPropertyChanged(nameof(QuanDaiLySeries));
        }


        #endregion


        #region hàm update
        private async Task UpdateLineChart()
        {
            // 1. Lấy tháng/năm hiện tại và năm trước
            int currentYear = _revenueChartYear;
            int lastYear = currentYear - 1;

            // 2. Khởi tạo labels & formatter
            MonthLabels = new[]
            {
                "Tháng 1","Tháng 2","Tháng 3","Tháng 4",
                "Tháng 5","Tháng 6","Tháng 7","Tháng 8",
                "Tháng 9","Tháng 10","Tháng 11","Tháng 12"
            };
            CurrencyFormatter = value => value.ToString("N0") + " đ";

            // 3. Lấy dữ liệu
            var allPhieuThu = await _phieuThuService
                .GetPhieuThuByCurrentYearAndLastYear(currentYear, lastYear);

            // 4. Tính tổng theo tháng
            var thisYearData = new double[12];
            var lastYearData = new double[12];
            foreach (var p in allPhieuThu)
            {
                int m = p.NgayThuTien.Month;
                if (p.NgayThuTien.Year == currentYear) thisYearData[m - 1] += p.SoTienThu;
                else if (p.NgayThuTien.Year == lastYear) lastYearData[m - 1] += p.SoTienThu;
            }

            // 5. Tính bước trục Y: chia đều thành 5 đoạn
            double maxThis = thisYearData.Any() ? thisYearData.Max() : 0;
            double maxLast = lastYearData.Any() ? lastYearData.Max() : 0;
            double maxAll = Math.Max(maxThis, maxLast);
            double rawStep = maxAll / 5.0;
            if (rawStep <= 0)
            {
                // không có dữ liệu ⇒ mặc định 1 triệu
                YAxisStepLineChart = 1_000_000;
            }
            else
            {
                // xác định bậc lớn nhất, ví dụ rawStep=4.68e6 ⇒ magnitude=1e6
                double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
                // làm tròn lên thành niceStep = ceil(rawStep/magnitude)*magnitude
                double niceStep = Math.Ceiling(rawStep / magnitude) * magnitude;
                YAxisStepLineChart = niceStep;
            }
            OnPropertyChanged(nameof(YAxisStepLineChart));

            // 6. Khởi tạo series
            SpendingSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title            = currentYear.ToString(),
                    Values           = new ChartValues<double>(thisYearData),
                    PointGeometry    = DefaultGeometries.Circle,
                    PointGeometrySize= 12,
                    LineSmoothness   = 0,
                    Stroke           = Brushes.Gray,
                    Fill             = Brushes.Transparent,
                    DataLabels       = false,
                    LabelPoint       = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness  = 3
                },
                new LineSeries
                {
                    Title            = lastYear.ToString(),
                    Values           = new ChartValues<double>(lastYearData),
                    PointGeometry    = DefaultGeometries.Square,
                    PointGeometrySize= 12,
                    LineSmoothness   = 1,
                    Stroke           = Brushes.DodgerBlue,
                    Fill             = Brushes.Transparent,
                    DataLabels       = false,
                    LabelPoint       = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness  = 2
                }
            };
            OnPropertyChanged(nameof(SpendingSeries));
        }

        private async Task UpdatePieChart()
        {
            // Lấy tháng/năm đang chọn
            int selectedMonth = GetSelectedMonthNumber(_pieChartMonth); // ví dụ từ "Tháng 4" => 4
            int selectedYear = _pieChartYear;
            var endDate = new DateTime(selectedYear, selectedMonth,
                DateTime.DaysInMonth(selectedYear, selectedMonth));


            // 2. Lấy toàn bộ loại đại lý
            var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();

            // 3. Lấy toàn bộ đại lý từ khi tạo app
            var allDaiLy = await _daiLyService.GetAllDaiLy();

            // 4. Lọc đại lý có NgayTiepNhan <= endDate và nhóm theo loại
            var daiLyCounts = allDaiLy
                .Where(d => d.NgayTiepNhan <= endDate)
                .GroupBy(d => d.MaLoaiDaiLy)
                .ToDictionary(g => g.Key, g => g.Count());


            // 5. Xây SeriesCollection cho PieChart
            DaiLyDistributionSeries = new SeriesCollection();
            foreach (var loai in listLoaiDaiLy)
            {
                daiLyCounts.TryGetValue(loai.MaLoaiDaiLy, out int count);

                DaiLyDistributionSeries.Add(new PieSeries
                {
                    Title = $"{loai.TenLoaiDaiLy} ({count})",
                    Values = new ChartValues<double> { count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = RandomColorBrush(loai.MaLoaiDaiLy)
                });
            }

            // 6. Notify UI
            OnPropertyChanged(nameof(DaiLyDistributionSeries));
        }

        private async Task UpdateDoanhSoChart()
        {
            await InitializeDoanhSoChart();
        }

        private async Task UpdateQuanPieChart()
        {
            await InitializeQuanPieChart();
        }
        private async Task UpdateDebtColumnChart()
        {
            // 1. Lấy tháng/năm đang chọn
            int selectedMonth = GetSelectedMonthNumber(_debtChartMonth);
            int selectedYear = _debtChartYear;

            DateTime startDate = new DateTime(selectedYear, selectedMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // 2. Lấy danh sách phiếu xuất và phiếu thu trong khoảng thời gian đó
            var phieuXuats = await _phieuXuatService.GetPhieuXuatByDateRange(startDate, endDate);
            var phieuThus = await _phieuThuService.GetPhieuThuByDateRange(startDate, endDate);

            // 3. Tính tổng phiếu xuất theo đại lý
            var tongXuatTheoDaiLy = phieuXuats
                .GroupBy(p => p.DaiLy)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.TongTriGia)
                );

            // 4. Tính tổng phiếu thu theo đại lý
            var tongThuTheoDaiLy = phieuThus
                .GroupBy(p => p.DaiLy)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.SoTienThu)
                );

            // 5. Lấy toàn bộ đại lý
            var allDaiLys = await _daiLyService.GetAllDaiLy();

            // 6. Tính công nợ
            var congNoTheoDaiLy = allDaiLys
                .Select(daiLy =>
                {
                    double tongXuat = tongXuatTheoDaiLy.ContainsKey(daiLy) ? tongXuatTheoDaiLy[daiLy] : 0;
                    double tongThu = tongThuTheoDaiLy.ContainsKey(daiLy) ? tongThuTheoDaiLy[daiLy] : 0;
                    return new { daiLy.TenDaiLy, TienNo = tongXuat - tongThu };
                })
                .Where(d => d.TienNo >= 0)
                .OrderByDescending(d => d.TienNo)
                .Take(10)
                .ToList();

            // 7. Cập nhật nhãn và giá trị cho biểu đồ
            var debtNames = congNoTheoDaiLy.Select(d => d.TenDaiLy).ToList();
            var debtValues = congNoTheoDaiLy.Select(d => d.TienNo).ToList();

            TopDebtDaiLyLabels = debtNames.ToArray();
            OnPropertyChanged(nameof(TopDebtDaiLyLabels));

            // 8. Tính bước trục Y (YAxisStepDebtColumnChart)
            double maxValue = debtValues.Any() ? debtValues.Max() : 0;
            double rawStep = maxValue / 5.0;
            if (rawStep <= 0)
            {
                YAxisStepDebtColumnChart = 1_000_000;
            }
            else
            {
                double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
                YAxisStepDebtColumnChart = Math.Ceiling(rawStep / magnitude) * magnitude;
            }
            OnPropertyChanged(nameof(YAxisStepDebtColumnChart));

            // 9. Cập nhật series cho biểu đồ
            TopDebtDaiLySeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Công nợ",
                    Values = new ChartValues<double>(debtValues),
                    DataLabels = true,
                    LabelPoint = p => p.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)),
                    MaxColumnWidth = 50
                }
            };

            OnPropertyChanged(nameof(TopDebtDaiLySeries));
        }

        // Widget
        private async Task UpdateWidgetDoanhThuTrongNgay()
        {
            // 1. Lấy tháng/năm hiện tại và ngày hiện tại
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            DateTime currentDay = currentDate;

            var selectedMonth = GetSelectedMonthNumber(_revenueChartMonth);
            var selectedYear = _revenueChartYear;
            // Kiểm tra nếu tháng hoặc năm đã thay đổi so với giá trị đã chọn
            if (currentMonth != selectedMonth || currentYear != selectedYear)
            {
                // Nếu tháng hoặc năm đã thay đổi, lấy ngày cuối của tháng đã chọn
                currentDay = new DateTime(selectedYear, selectedMonth, DateTime.DaysInMonth(selectedYear, selectedMonth));
            }

            // 2. Lấy tổng doanh thu cho ngày hiện tại từ dịch vụ
            var currTotal = await _phieuThuService.GetToltalPhieuThuBySingleDate(currentDay);

            // 3. Lấy tổng doanh thu cho ngày hôm qua
            DateTime prevDate = currentDay.AddDays(-1);
            var prevTotal = await _phieuThuService.GetToltalPhieuThuBySingleDate(prevDate);

            // 4. Tính % thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu ngày trước không có doanh thu, mặc định lên 100% nếu có doanh thu mới
                deltaPercent = currTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 5. Gán giá trị cho widget
            WidgetDoanhThuTrongNgay = currTotal.ToString("N0") + " đ";  // Hiển thị doanh thu của ngày hiện tại
            WidgetDoanhThuTrongNgayDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi
            WidgetDoanhThuTrongNgayIsUp = deltaPercent >= 0;

            OnPropertyChanged(nameof(WidgetDoanhThuTrongNgay));
            OnPropertyChanged(nameof(WidgetDoanhThuTrongNgayDeltaText));
            OnPropertyChanged(nameof(WidgetDoanhThuTrongNgayIsUp));
        }
        private async Task UpdateWidgetDoanhThuThang()
        {
            // 1. Lấy tháng/năm hiện tại và tháng/năm trước
            var currentMonth = GetSelectedMonthNumber(_revenueChartMonth);
            var currentYear = _revenueChartYear;
            int prevMonth, prevYear;

            if (currentMonth == 1)
            {
                prevMonth = 12;
                prevYear = currentYear - 1;
            }
            else
            {
                prevMonth = currentMonth - 1;
                prevYear = currentYear;
            }

            // 2. Lấy tổng doanh thu cho tháng này và tháng trước
            long currTotal = await _phieuThuService.GetTotalPhieuThuUpToMonthYear(currentMonth, currentYear);
            long prevTotal = await _phieuThuService.GetTotalPhieuThuUpToMonthYear(prevMonth, prevYear);

            // 3. Tính phần trăm thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu tháng trước không có doanh thu, mặc định tăng 100% nếu có doanh thu mới
                deltaPercent = currTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 4. Gán giá trị cho widget
            WidgetDoanhThuThang = currTotal.ToString("N0") + " đ";  // Hiển thị tổng doanh thu của tháng hiện tại
            WidgetDoanhThuThangDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi
            WidgetDoanhThuThangIsUp = deltaPercent >= 0;
        }

        private async Task UpdateWidgetDoanhThuTrongNam()
        {
            // 1. Lấy năm hiện tại
            var currentYear = _revenueChartYear;

            // 2. Lấy tổng doanh thu cho năm hiện tại
            long currTotal = await _phieuThuService.GetTotalPhieuThuByYear(currentYear);

            // 3. Lấy tổng doanh thu cho năm trước
            int prevYear = currentYear - 1;
            long prevTotal = await _phieuThuService.GetTotalPhieuThuByYear(prevYear);

            // 4. Tính phần trăm thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu năm trước không có doanh thu, mặc định tăng 100% nếu có doanh thu mới
                deltaPercent = currTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 5. Gán giá trị cho widget
            WidgetDoanhThuTrongNam = currTotal.ToString("N0") + " đ";  // Hiển thị tổng doanh thu của năm hiện tại
            WidgetDoanhThuTrongNamDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi
            WidgetDoanhThuTrongNamIsUp = deltaPercent >= 0;
        }

        private async Task UpdateWidgetDoanhSoTrongNgay()
        {
            // 1. Lấy tháng/năm hiện tại và ngày hiện tại
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            DateTime currentDay = currentDate;

            var selectedMonth = GetSelectedMonthNumber(_DoanhSoChartMonth);
            var selectedYear = _DoanhSoChartYear;
            // Kiểm tra nếu tháng hoặc năm đã thay đổi so với giá trị đã chọn
            if (currentMonth != selectedMonth || currentYear != selectedYear)
            {
                // Nếu tháng hoặc năm đã thay đổi, lấy ngày cuối của tháng đã chọn
                currentDay = new DateTime(selectedYear, selectedMonth, DateTime.DaysInMonth(selectedYear, selectedMonth));
            }

            // 2. Lấy tổng doanh số cho ngày hiện tại từ dịch vụ
            var currTotal = await _phieuXuatService.GetToltalPhieuXuatBySingleDate(currentDay);

            // 3. Lấy tổng doanh số cho ngày hôm qua
            DateTime prevDate = currentDay.AddDays(-1);
            var prevTotal = await _phieuXuatService.GetToltalPhieuXuatBySingleDate(prevDate);

            // 4. Tính % thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu ngày trước không có doanh số, mặc định lên 100% nếu có doanh số mới
                deltaPercent = currTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 5. Gán giá trị cho widget
            WidgetDoanhSoTrongNgay = currTotal.ToString("N0") + " đ";  // Hiển thị doanh số của ngày hiện tại
            WidgetDoanhSoTrongNgayDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi
            WidgetDoanhSoTrongNgayIsUp = deltaPercent >= 0;

        }
        private async Task UpdateWidgetDoanhSoTrongThang()
        {
            // 1. Lấy tháng/năm hiện tại và tháng/năm trước
            var currentMonth = GetSelectedMonthNumber(_DoanhSoChartMonth);
            var currentYear = _DoanhSoChartYear;
            int prevMonth, prevYear;

            if (currentMonth == 1)
            {
                prevMonth = 12;
                prevYear = currentYear - 1;
            }
            else
            {
                prevMonth = currentMonth - 1;
                prevYear = currentYear;
            }

            // 2. Lấy tổng doanh số cho tháng này và tháng trước
            long currTotal = await _phieuXuatService.GetTotalPhieuXuatByCurrentMonthYear(currentMonth, currentYear);
            long prevTotal = await _phieuXuatService.GetTotalPhieuXuatByCurrentMonthYear(prevMonth, prevYear);

            // 3. Tính phần trăm thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu tháng trước không có doanh số, mặc định tăng 100% nếu có doanh số mới
                deltaPercent = currTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 4. Gán giá trị cho widget
            WidgetDoanhSoTrongThang = currTotal.ToString("N0") + " đ";  // Hiển thị tổng doanh số của tháng hiện tại
            WidgetDoanhSoTrongThangDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi
            WidgetDoanhSoTrongThangIsUp = deltaPercent >= 0;
        }

        private async Task UpdateWidgetDoanhSoTrongNam()
        {
            // 1. Lấy năm hiện tại
            var currentYear = _DoanhSoChartYear;

            // 2. Lấy tổng doanh số cho năm hiện tại
            long currTotal = await _phieuXuatService.GetTotalPhieuXuatByYear(currentYear);

            // 3. Lấy tổng doanh số cho năm trước
            int prevYear = currentYear - 1;
            long prevTotal = await _phieuXuatService.GetTotalPhieuXuatByYear(prevYear);

            // 4. Tính phần trăm thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu năm trước không có doanh số, mặc định tăng 100% nếu có doanh số mới
                deltaPercent = currTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 5. Gán giá trị cho widget
            WidgetDoanhSoTrongNam = currTotal.ToString("N0") + " đ";  // Hiển thị tổng doanh số của năm hiện tại
            WidgetDoanhSoTrongNamDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi
            WidgetDoanhSoTrongNamIsUp = deltaPercent >= 0;
        }

        private async Task UpdateWidgetTongCongNoDauThang()
        {
            // 1. Lấy tháng/năm hiện tại và ngày đầu tháng
            var currentMonth = GetSelectedMonthNumber(_debtChartMonth);
            var currentYear = _debtChartYear;

            // 1. Lấy ngày đầu tiên có đại lý (ngày sớm nhất)
            var earliest = await _daiLyService.GetEarliestDaiLyDateAsync();
            DateTime startDate = earliest ?? new DateTime(currentYear, currentMonth, 1);  // Nếu không có đại lý, mặc định ngày đầu tháng hiện tại

            // Ngày đầu tháng hiện tại
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1);

            // 2. Lấy tổng doanh số xuất từ lúc lập app đến ngày đầu tháng
            var phieuXuatList = await _phieuXuatService.GetPhieuXuatByDateRange(startDate, firstDayOfMonth);
            long totalXuat = phieuXuatList.Sum(p => p.TongTriGia);  // Tính tổng doanh số xuất

            // 3. Lấy tổng doanh thu (thu) từ lúc lập app đến ngày đầu tháng
            var phieuThuList = await _phieuThuService.GetPhieuThuByDateRange(startDate, firstDayOfMonth);
            long totalThu = phieuThuList.Sum(p => p.SoTienThu);  // Tính tổng doanh thu thu

            // 4. Tính tổng công nợ đầu tháng
            long totalCongNoDauThang = totalXuat - totalThu;

            // 5. Lấy tổng công nợ đầu tháng của tháng trước
            DateTime firstDayOfPreviousMonth = new DateTime(currentYear, currentMonth - 1, 1);
            if (currentMonth == 1) // Đặc biệt đối với tháng 1, tháng trước sẽ là tháng 12 của năm trước
            {
                firstDayOfPreviousMonth = new DateTime(currentYear - 1, 12, 1);
            }

            // Lấy tổng doanh số và thu cho tháng trước
            var phieuXuatListPrevMonth = await _phieuXuatService.GetPhieuXuatByDateRange(startDate, firstDayOfPreviousMonth);
            long totalXuatPrevMonth = phieuXuatListPrevMonth.Sum(p => p.TongTriGia);
            var phieuThuListPrevMonth = await _phieuThuService.GetPhieuThuByDateRange(startDate, firstDayOfPreviousMonth);
            long totalThuPrevMonth = phieuThuListPrevMonth.Sum(p => p.SoTienThu);

            // Tính công nợ đầu tháng của tháng trước
            long totalCongNoDauThangPrevMonth = totalXuatPrevMonth - totalThuPrevMonth;

            // 6. Tính % thay đổi công nợ so với tháng trước
            double deltaPercent = 0;
            if (totalCongNoDauThangPrevMonth == 0 && totalCongNoDauThang > 0)
            {
                deltaPercent = 100; // Nếu tháng trước không có công nợ, mặc định tăng 100%
            }
            else if (totalCongNoDauThangPrevMonth > 0)
            {
                deltaPercent = (totalCongNoDauThang - totalCongNoDauThangPrevMonth) * 100.0 / totalCongNoDauThangPrevMonth;
            }

            // 7. Cập nhật các widget
            WidgetTongCongNoDauThang = totalCongNoDauThang.ToString("N0") + " đ";  // Hiển thị tổng công nợ đầu tháng

            // 8. Hiển thị % thay đổi
            WidgetTongCongNoDauThangDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi

            // 9. Kiểm tra nếu công nợ tăng hay giảm
            WidgetTongCongNoDauThangIsUp = deltaPercent >= 0;
        }

        private async Task UpdateWidgetTongNoCuoiThang()
        {
            // 1. Lấy tháng/năm hiện tại và ngày đầu tháng
            var currentMonth = GetSelectedMonthNumber(_debtChartMonth);
            var currentYear = _debtChartYear;

            var date = DateTime.Now;
            if (currentMonth == date.Month && currentYear == date.Year)
            {
                WidgetTongNoCuoiThangText = "Tổng công nợ hiện tại";
            }
            else
            {
                WidgetTongNoCuoiThangText = "Tổng công nợ cuối tháng";
            }    
            // Lấy ngày đầu tháng hiện tại
            DateTime firstDayOfMonth = new DateTime(currentYear, currentMonth, 1);

            // Lấy ngày cuối tháng hiện tại
            DateTime lastDayOfMonth = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));

            // 2. Lấy giá trị công nợ đầu tháng từ WidgetTongCongNoDauThang
            long totalCongNoDauThang = long.TryParse(WidgetTongCongNoDauThang.Replace(" đ", "").Replace(",", ""), out var result) ? result : 0;

            // 4. Lấy tổng doanh số xuất trong tháng
            var phieuXuatListInMonth = await _phieuXuatService.GetPhieuXuatByDateRange(firstDayOfMonth, lastDayOfMonth);
            long totalXuatInMonth = phieuXuatListInMonth.Sum(p => p.TongTriGia);  // Tính tổng doanh số xuất trong tháng

            // 5. Lấy tổng doanh thu thu trong tháng
            var phieuThuListInMonth = await _phieuThuService.GetPhieuThuByDateRange(firstDayOfMonth, lastDayOfMonth);
            long totalThuInMonth = phieuThuListInMonth.Sum(p => p.SoTienThu);  // Tính tổng doanh thu thu trong tháng

            // 6. Tính tổng công nợ cuối tháng
            long totalCongNoCuoiThang = totalCongNoDauThang + totalXuatInMonth - totalThuInMonth;

            // Lấy tổng doanh số và thu cho tháng trước
            var totalXuatPrevMonth = await _phieuXuatService.GetToltalPhieuXuatBySingleDate(firstDayOfMonth);
            var totalThuPrevMonth = await _phieuThuService.GetToltalPhieuThuBySingleDate(firstDayOfMonth);

            // Tính công nợ cuối tháng của tháng trước
            long totalCongNoCuoiThangPrevMonth = totalCongNoDauThang - totalXuatPrevMonth + totalThuPrevMonth;

            // 8. Tính % thay đổi công nợ so với tháng trước
            double deltaPercentCuoiThang = 0;
            if (totalCongNoCuoiThangPrevMonth == 0 && totalCongNoCuoiThang > 0)
            {
                deltaPercentCuoiThang = 100; // Nếu tháng trước không có công nợ, mặc định tăng 100%
            }
            else if (totalCongNoCuoiThangPrevMonth > 0)
            {
                deltaPercentCuoiThang = (totalCongNoCuoiThang - totalCongNoCuoiThangPrevMonth) * 100.0 / totalCongNoCuoiThangPrevMonth;
            }

            // 9. Cập nhật widget công nợ cuối tháng
            WidgetTongCongNoCuoiThang = totalCongNoCuoiThang.ToString("N0") + " đ";  // Hiển thị tổng công nợ cuối tháng

            // 10. Hiển thị % thay đổi công nợ cuối tháng
            WidgetTongCongNoCuoiThangDeltaText = (deltaPercentCuoiThang >= 0 ? "+" : "") + deltaPercentCuoiThang.ToString("0.#") + "%";  // Hiển thị % thay đổi

            // 11. Kiểm tra nếu công nợ tăng hay giảm
            WidgetTongCongNoCuoiThangIsUp = deltaPercentCuoiThang >= 0;

        }

        private async Task UpdateWidgetSoLuongDaiLyDangNo()
        {
            // 1. Lấy tháng/năm hiện tại và ngày đầu tháng
            var currentMonth = GetSelectedMonthNumber(_debtChartMonth);
            var currentYear = _debtChartYear;

            // 1. Lấy ngày đầu tiên có đại lý (ngày sớm nhất)
            var earliest = await _daiLyService.GetEarliestDaiLyDateAsync();
            DateTime startDate = earliest ?? new DateTime(currentYear, currentMonth, 1);  // Nếu không có đại lý, mặc định ngày đầu tháng hiện tại

            // Ngày cuối tháng hiện tại
            DateTime lastDayOfMonth = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));



            // 2. Lấy danh sách phiếu xuất và phiếu thu trong khoảng thời gian của tháng hiện tại
            var phieuXuats = await _phieuXuatService.GetPhieuXuatByDateRange(startDate, lastDayOfMonth);
            var phieuThus = await _phieuThuService.GetPhieuThuByDateRange(startDate, lastDayOfMonth);

            // 3. Tính tổng phiếu xuất theo đại lý
            var tongXuatTheoDaiLy = phieuXuats
                .GroupBy(p => p.DaiLy)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.TongTriGia)  // Tổng doanh số xuất
                );

            // 4. Tính tổng phiếu thu theo đại lý
            var tongThuTheoDaiLy = phieuThus
                .GroupBy(p => p.DaiLy)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.SoTienThu)  // Tổng doanh thu thu
                );

            // 5. Lấy toàn bộ đại lý
            var allDaiLys = await _daiLyService.GetAllDaiLy();

            // 6. Tính công nợ và đếm số lượng đại lý đang nợ trong tháng hiện tại
            var daiLyDangNoCountCurrentMonth = allDaiLys
                .Select(daiLy =>
                {
                    // Tính tổng xuất và thu cho đại lý này
                    double tongXuat = tongXuatTheoDaiLy.ContainsKey(daiLy) ? tongXuatTheoDaiLy[daiLy] : 0;
                    double tongThu = tongThuTheoDaiLy.ContainsKey(daiLy) ? tongThuTheoDaiLy[daiLy] : 0;
                    double tienNo = tongXuat - tongThu;

                    return new { daiLy.TenDaiLy, TienNo = tienNo };
                })
                .Where(d => d.TienNo > 0)  // Chỉ những đại lý có nợ (TienNo > 0)
                .Count();  // Đếm số lượng đại lý đang nợ

            DateTime lastDayOfPreviousMonth;
            if (currentMonth == 1)
            {
                // Nếu tháng hiện tại là tháng 1, tháng trước là tháng 12 của năm trước
                lastDayOfPreviousMonth = new DateTime(currentYear - 1, 12, DateTime.DaysInMonth(currentYear - 1, 12));
            }
            else
            {
                // Nếu tháng không phải tháng 1, lấy ngày cuối tháng trước
                lastDayOfPreviousMonth = new DateTime(currentYear, currentMonth, 1).AddDays(-1);
            }
            // 7. Lấy số lượng đại lý đang nợ trong tháng trước
            var phieuXuatsPrevMonth = await _phieuXuatService.GetPhieuXuatByDateRange(startDate, lastDayOfPreviousMonth);
            var phieuThusPrevMonth = await _phieuThuService.GetPhieuThuByDateRange(startDate, lastDayOfPreviousMonth);

            // Tính tổng phiếu xuất và phiếu thu trong tháng trước
            var tongXuatTheoDaiLyPrevMonth = phieuXuatsPrevMonth
                .GroupBy(p => p.DaiLy)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.TongTriGia));

            var tongThuTheoDaiLyPrevMonth = phieuThusPrevMonth
                .GroupBy(p => p.DaiLy)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.SoTienThu));

            var daiLyDangNoCountPrevMonth = allDaiLys
                .Select(daiLy =>
                {
                    double tongXuatPrevMonth = tongXuatTheoDaiLyPrevMonth.ContainsKey(daiLy) ? tongXuatTheoDaiLyPrevMonth[daiLy] : 0;
                    double tongThuPrevMonth = tongThuTheoDaiLyPrevMonth.ContainsKey(daiLy) ? tongThuTheoDaiLyPrevMonth[daiLy] : 0;
                    double tienNoPrevMonth = tongXuatPrevMonth - tongThuPrevMonth;

                    return new { daiLy.TenDaiLy, TienNoPrevMonth = tienNoPrevMonth };
                })
                .Where(d => d.TienNoPrevMonth > 0)  // Chỉ những đại lý có nợ (TienNoPrevMonth > 0)
                .Count();  // Đếm số lượng đại lý đang nợ trong tháng trước

            // 8. Tính % thay đổi số lượng đại lý đang nợ so với tháng trước
            double deltaPercent = 0;
            if (daiLyDangNoCountPrevMonth == 0 && daiLyDangNoCountCurrentMonth > 0)
            {
                deltaPercent = 100; // Nếu tháng trước không có đại lý đang nợ, mặc định tăng 100%
            }
            else if (daiLyDangNoCountPrevMonth > 0)
            {
                deltaPercent = (daiLyDangNoCountCurrentMonth - daiLyDangNoCountPrevMonth) * 100.0 / daiLyDangNoCountPrevMonth;
            }

            // 9. Cập nhật widget số lượng đại lý đang nợ
            WidgetSoLuongDaiLyDangNo = daiLyDangNoCountCurrentMonth.ToString();  // Hiển thị số lượng đại lý đang nợ

            // 10. Hiển thị % thay đổi
            WidgetSoLuongDaiLyDangNoDeltaText = (deltaPercent >= 0 ? "+" : "") + deltaPercent.ToString("0.#") + "%";  // Hiển thị % thay đổi

            // 11. Kiểm tra nếu số lượng đại lý tăng hay giảm
            WidgetSoLuongDaiLyDangNoIsUp = deltaPercent >= 0;
        }


        #endregion

        #region Binding Properties

        // Widget properties
        [ObservableProperty]
        private string _WidgetTongCongNoDauThang = "0 đ";
        [ObservableProperty]
        private string _WidgetTongCongNoDauThangDeltaText = "0%";
        [ObservableProperty]
        private bool _WidgetTongCongNoDauThangIsUp = true;

        [ObservableProperty]
        private string _WidgetTongCongNoCuoiThang = "0 đ";
        [ObservableProperty]
        private string _WidgetTongCongNoCuoiThangDeltaText = "0%";
        [ObservableProperty]
        private bool _WidgetTongCongNoCuoiThangIsUp = true;
        [ObservableProperty]
        private string _WidgetTongNoCuoiThangText= "";

        [ObservableProperty]
        private string _WidgetSoLuongDaiLyDangNo = "0";
        [ObservableProperty]
        private string _WidgetSoLuongDaiLyDangNoDeltaText = "0%";
        [ObservableProperty]
        private bool _WidgetSoLuongDaiLyDangNoIsUp = true;

        [ObservableProperty]
        private string _WidgetDoanhSoTrongNgay = "0 đ";
        [ObservableProperty]
        private string _WidgetDoanhSoTrongNgayDeltaText = "0%";
        [ObservableProperty]
        private bool _WidgetDoanhSoTrongNgayIsUp = true;

        [ObservableProperty]
        private string _WidgetDoanhSoTrongThang = "0 đ";
        [ObservableProperty]
        private string _WidgetDoanhSoTrongThangDeltaText = "0%";
        [ObservableProperty]
        private bool _WidgetDoanhSoTrongThangIsUp = true;

        [ObservableProperty]
        private string _WidgetDoanhSoTrongNam = "0 đ";
        [ObservableProperty]
        private string _WidgetDoanhSoTrongNamDeltaText = "0%";
        [ObservableProperty]
        private bool _WidgetDoanhSoTrongNamIsUp = true;


        [ObservableProperty]
        private string _WidgetDoanhThuTrongNamDeltaText = "0";

        [ObservableProperty]
        private bool _WidgetDoanhThuTrongNamIsUp = true;

        [ObservableProperty]
        private string _WidgetDoanhThuTrongNam = "";

        [ObservableProperty]
        private string _WidgetDoanhThuTrongNgayDeltaText = "0";

        [ObservableProperty]
        private bool _WidgetDoanhThuTrongNgayIsUp = true;

        [ObservableProperty]
        private string _WidgetDoanhThuTrongNgay = "";

        [ObservableProperty]
        private string _WidgetDoanhThuThangDeltaText = "0";

        [ObservableProperty]
        private bool _WidgetDoanhThuThangIsUp = true;

        [ObservableProperty]
        private string _WidgetDoanhThuThang = "";



        [ObservableProperty]
        private string _widgetPhieuThuDeltaText = "0%";

        [ObservableProperty]
        private bool _widgetPhieuThuIsUp = true;

        [ObservableProperty]
        private string _widgetPhieuXuatDeltaText = "0%";

        [ObservableProperty]
        private bool _widgetPhieuXuatIsUp = true;

        [ObservableProperty]
        private string _WidgetTongGiaTriPhieuThu = "0 đ";

        [ObservableProperty]
        private string _WidgetTongGiaTriPhieuXuat = "0 đ";




        [ObservableProperty]
        private string _DoanhSoChartMonth= "Tháng 1";

        [ObservableProperty]
        private int _DoanhSoChartYear = 1;

        [ObservableProperty]
        private double _yAxisStepLineChart;

        [ObservableProperty]
        private double _yAxisStepDoanhSoChart;

        [ObservableProperty]
        private double _yAxisStepColumnChart;

        [ObservableProperty]
        private double _yAxisStepDebtColumnChart;

        [ObservableProperty]
        private string _revenueChartMonth = "Tháng 1";

        [ObservableProperty]
        private int _revenueChartYear = 1;

        [ObservableProperty]
        private string _pieChartMonth = "";

        [ObservableProperty]
        private int _pieChartYear = 0;

        [ObservableProperty]
        private string _topAgentChartMonth = "";

        [ObservableProperty]
        private int _topAgentChartYear = 0;

        [ObservableProperty]
        private string _debtChartMonth = "";

        [ObservableProperty]
        private int _debtChartYear = 0;

        // For backward compatibility
        [ObservableProperty]
        private string _selectedMonth = "";

        [ObservableProperty]
        private int _selectedYear = 0;

        #endregion

        #region Partial Methods
        partial void OnRevenueChartMonthChanged(string value)
        {
            _ = Task.WhenAll(
                UpdateLineChart(),
                UpdateWidgetDoanhThuTrongNgay(),
                UpdateWidgetDoanhThuThang(),
                UpdateWidgetDoanhThuTrongNam()
                );
        }

        // Khi năm thay đổi
        partial void OnRevenueChartYearChanged(int value)
        {
            _ = Task.WhenAll(
                UpdateLineChart(),
                UpdateWidgetDoanhThuTrongNgay(),
                UpdateWidgetDoanhThuThang(),
                UpdateWidgetDoanhThuTrongNam()
                );
        }

        // Khi tháng trong biểu đồ cột thay đổi
        partial void OnPieChartMonthChanged(string value)
        {
            _ = Task.WhenAll(
                UpdatePieChart(),
                UpdateQuanPieChart()
                );
        }

        // Khi năm trong biểu đồ cột thay đổi
        partial void OnPieChartYearChanged(int value)
        {
            _ = Task.WhenAll(
                UpdatePieChart(),
                UpdateQuanPieChart()
                );
        }

        // Khi tháng của công nợ thay đổi
        partial void OnDebtChartMonthChanged(string value)
        {
            _ = Task.WhenAll(
                UpdateDebtColumnChart(),
                UpdateWidgetDebtChart(),
                UpdateWidgetSoLuongDaiLyDangNo()
                );
            
        }

        private async Task UpdateWidgetDebtChart()
        {
            // Gọi UpdateWidgetTongCongNoDauThang và đợi cho đến khi hoàn tất
            await UpdateWidgetTongCongNoDauThang();

            // Sau khi UpdateWidgetTongCongNoDauThang hoàn tất, gọi UpdateWidgetTongNoCuoiThang
            await UpdateWidgetTongNoCuoiThang();
        }
        // Khi năm của công nợ thay đổi
        partial void OnDebtChartYearChanged(int value)
        {
            _ = Task.WhenAll
                (
                    UpdateDebtColumnChart(),
                    UpdateWidgetDebtChart(),
                    UpdateWidgetSoLuongDaiLyDangNo()
                );
        }

        partial void OnDoanhSoChartMonthChanged(string value)
        {
            _ = Task.WhenAll(
                UpdateDoanhSoChart(),
                UpdateWidgetDoanhSoTrongNgay(),
                UpdateWidgetDoanhSoTrongThang(),
                UpdateWidgetDoanhSoTrongNam()
                );
        }

        partial void OnDoanhSoChartYearChanged(int value)
        {
            _ = Task.WhenAll(
                    UpdateDoanhSoChart(),
                    UpdateWidgetDoanhSoTrongNgay(),
                    UpdateWidgetDoanhSoTrongThang(),
                    UpdateWidgetDoanhSoTrongNam()
                    );
        }
        // Khi tháng được chọn thay đổi (cho tất cả các biểu đồ tháng)
        partial void OnSelectedMonthChanged(string value)
        {
            // Sync all chart-specific month properties
            RevenueChartMonth = value;
            PieChartMonth = value;
            TopAgentChartMonth = value;
            DebtChartMonth = value;
            DoanhSoChartMonth = value;
        }

        // Khi năm được chọn thay đổi (cho tất cả các biểu đồ năm)
        partial void OnSelectedYearChanged(int value)
        {
            // Sync all chart-specific year properties
            RevenueChartYear = value;
            PieChartYear = value;
            TopAgentChartYear = value;
            DebtChartYear = value;
            DoanhSoChartYear = value;
        }

        #endregion
    }

}