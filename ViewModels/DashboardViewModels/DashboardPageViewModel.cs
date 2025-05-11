using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using QuanLyDaiLy.Services;

namespace QuanLyDaiLy.ViewModels.DashboardViewModels
{
    public class DashboardPageViewModel : INotifyPropertyChanged
    {
        public SeriesCollection SpendingSeries { get; set; } = [];
        public SeriesCollection DaiLyDistributionSeries { get; set; } = [];
        public string[] MonthLabels { get; set; } = null!;
        public Func<double, string> CurrencyFormatter { get; set; } = value => value.ToString("N0") + " đ";
        public SeriesCollection TopDaiLySeries { get; set; } = [];
        public string[] TopDaiLyLabels { get; set; } = null!;
        public SeriesCollection TopDebtDaiLySeries { get; set; } = [];
        public SeriesCollection DoanhSoSeries { get; set; } = [];
        public SeriesCollection QuanDaiLySeries { get; set; } = [];
        public string[] TopDebtDaiLyLabels { get; set; } = null!;
        public List<string> MonthOptions { get; set; } = [];
        public List<int> YearOptions { get; set; } = [];

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


            _ = InitializeMonthYearOptions();
            _ = InitializeLineChart();
            _ = InitializePieChart();
            _ = InitializeColumnChart();
            _ = InitializeDebtColumnChart();
            _ = UpdateWidgetSoLuongDaiLy();
            _ = UpdateWidgetDoanhThuThang();
            _ = UpdateWidgetTongGiaTriPhieuThu();
            _ = UpdateWidgetTongGiaTriPhieuXuat();
            _ = InitializeDoanhSoChart();
            _ = InitializeQuanPieChart();
        }

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
        }

        private async Task InitializeDoanhSoChart()
        {
            // Generate random data for current year and previous year
            var random = new Random();
            var thisYearData = new double[12];
            var lastYearData = new double[12];

            // Random values between 5-25 million
            for (int i = 0; i < 12; i++)
            {
                thisYearData[i] = random.Next(5_000_000, 25_000_000);
                lastYearData[i] = random.Next(5_000_000, 25_000_000);
            }

            // Calculate Y-axis step (reusing the approach from other charts)
            double maxThis = thisYearData.Max();
            double maxLast = lastYearData.Max();
            double maxAll = Math.Max(maxThis, maxLast);
            double rawStep = maxAll / 5.0;
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
            double niceStep = Math.Ceiling(rawStep / magnitude) * magnitude;

            // Use the dedicated YAxisStepDoanhSoChart property instead of YAxisStepLineChart
            YAxisStepDoanhSoChart = niceStep;
            OnPropertyChanged(nameof(YAxisStepDoanhSoChart));

            // Create the series for the chart
            DoanhSoSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"{DateTime.Now.Year} (Doanh số)",
                    Values = new ChartValues<double>(thisYearData),
                    PointGeometry = DefaultGeometries.Diamond,
                    PointGeometrySize = 12,
                    LineSmoothness = 0,
                    Stroke = new SolidColorBrush(Color.FromRgb(76, 175, 80)), // Green
                    Fill = Brushes.Transparent,
                    DataLabels = false,
                    LabelPoint = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness = 3
                },
                new LineSeries
                {
                    Title = $"{DateTime.Now.Year - 1} (Doanh số)",
                    Values = new ChartValues<double>(lastYearData),
                    PointGeometry = DefaultGeometries.Triangle,
                    PointGeometrySize = 12,
                    LineSmoothness = 1,
                    Stroke = new SolidColorBrush(Color.FromRgb(255, 152, 0)), // Orange
                    Fill = Brushes.Transparent,
                    DataLabels = false,
                    LabelPoint = pt => pt.Y.ToString("N0") + " đ",
                    StrokeThickness = 2
                }
            };

            OnPropertyChanged(nameof(DoanhSoSeries));
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


        private async Task UpdateColumnChart()
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

            // 4. Lấy thông tin DaiLy cho top 5 trong một query
            var topIds = top5Dic.Select(kv => kv.Key).ToList();
            var daiLyList = await _daiLyService.GetDaiLysByIdsAsync(topIds);

            // 5. Chuẩn bị mảng tên và giá trị (theo đúng thứ tự top5Dic)
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

            // 6. Cập nhật nhãn trục X
            TopDaiLyLabels = dailyNames.ToArray();
            OnPropertyChanged(nameof(TopDaiLyLabels));

            // 7. Cập nhật series cho biểu đồ
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

        private async Task UpdateWidgetSoLuongDaiLy()
        {
            // 1. Xác định tháng/năm hiện tại và tháng/năm trước
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

            // 2. Lấy tổng đại lý từ khi tạo app đến hết tháng này và tháng trước
            long currCount = await _daiLyService.GetTotalDaiLyUpToMonthYear(currentMonth, currentYear);
            long prevCount = await _daiLyService.GetTotalDaiLyUpToMonthYear(prevMonth, prevYear);

            // 3. Tính % thay đổi
            double deltaPercent;
            if (prevCount == 0)
            {
                deltaPercent = currCount > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (currCount - prevCount) * 100.0 / prevCount;
            }

            // 4. Gán giá trị cho widget
            WidgetSoLuongDaiLy = currCount.ToString();
            WidgetSoLuongDaiLyDeltaText = (deltaPercent >= 0 ? "+" : "")
                                           + deltaPercent.ToString("0.#") + "%";
            WidgetSoLuongDaiLyIsUp = deltaPercent >= 0;

            // 5. Notify UI
            OnPropertyChanged(nameof(WidgetSoLuongDaiLy));
            OnPropertyChanged(nameof(WidgetSoLuongDaiLyDeltaText));
            OnPropertyChanged(nameof(WidgetSoLuongDaiLyIsUp));
        }

        private async Task UpdateWidgetDoanhThuThang()
        {
            var currentMonth = GetSelectedMonthNumber(_revenueChartMonth);
            var currentYear = _revenueChartYear;
            // Lấy danh sách phiếu thu theo tháng/năm
            var totalDoanhThu = await _phieuThuService.GetTotalPhieuThuUpToMonthYear(currentMonth, currentYear);

            // Cập nhật doanh thu vào WidgetDoanhThuThang
            WidgetDoanhThuThang = totalDoanhThu.ToString("N0") + " đ";  // Định dạng với dấu phẩy
            OnPropertyChanged(nameof(WidgetDoanhThuThang));
        }

        private async Task UpdateWidgetTongGiaTriPhieuThu()
        {
            // 1. Xác định tháng/năm hiện tại và tháng/năm trước
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

            // 2. Lấy tổng giá trị phiếu thu tháng này và tháng trước
            long thisTotal = await _phieuThuService
                .GetTotalPhieuThuByCurrentMonthYear(currentMonth, currentYear);
            long prevTotal = await _phieuThuService
                .GetTotalPhieuThuByCurrentMonthYear(prevMonth, prevYear);

            // 3. Tính % thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Nếu tháng trước không có doanh thu, mặc định lên 100% nếu có doanh thu mới
                deltaPercent = thisTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (thisTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 4. Gán giá trị cho widget
            WidgetTongGiaTriPhieuThu = thisTotal.ToString("N0") + " đ";
            WidgetPhieuThuDeltaText = (deltaPercent >= 0 ? "+" : "")
                                        + deltaPercent.ToString("0.#") + "%";
            WidgetPhieuThuIsUp = deltaPercent >= 0;

            // 5. Notify UI
            OnPropertyChanged(nameof(WidgetTongGiaTriPhieuThu));
            OnPropertyChanged(nameof(WidgetPhieuThuDeltaText));
            OnPropertyChanged(nameof(WidgetPhieuThuIsUp));
        }

        private async Task UpdateWidgetTongGiaTriPhieuXuat()
        {
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

            // 2. Lấy tổng giá trị phiếu xuất tháng này và tháng trước
            long thisTotal = await _phieuXuatService
                .GetTotalPhieuXuatByCurrentMonthYear(currentMonth, currentYear);
            long prevTotal = await _phieuXuatService
                .GetTotalPhieuXuatByCurrentMonthYear(prevMonth, prevYear);

            // 3. Tính % thay đổi
            double deltaPercent;
            if (prevTotal == 0)
            {
                // Chọn quy ước: nếu tháng trước = 0 thì luôn lên 100%
                deltaPercent = thisTotal > 0 ? 100 : 0;
            }
            else
            {
                deltaPercent = (thisTotal - prevTotal) * 100.0 / prevTotal;
            }

            // 4. Gán giá trị cho widget
            WidgetTongGiaTriPhieuXuat = thisTotal.ToString("N0") + " đ";
            WidgetPhieuXuatDeltaText = (deltaPercent >= 0 ? "+" : "")
                                            + deltaPercent.ToString("0.#") + "%";
            WidgetPhieuXuatIsUp = deltaPercent >= 0;

            OnPropertyChanged(nameof(WidgetTongGiaTriPhieuXuat));
            OnPropertyChanged(nameof(WidgetPhieuXuatDeltaText));
            OnPropertyChanged(nameof(WidgetPhieuXuatIsUp));
        }

        private async Task UpdateDoanhSoChart()
        {
            await InitializeDoanhSoChart();
        }

        private async Task UpdateQuanPieChart()
        {
            await InitializeQuanPieChart();
        }

        #endregion

        #region Binding Properties

        private double _yAxisStepLineChart;
        public double YAxisStepLineChart
        {
            get => _yAxisStepLineChart;
            set 
            {
                if (_yAxisStepLineChart != value)
                {
                    _yAxisStepLineChart = value;
                    OnPropertyChanged();
                }    
                    
            }
        }

        private double _yAxisStepDoanhSoChart;
        public double YAxisStepDoanhSoChart
        {
            get => _yAxisStepDoanhSoChart;
            set
            {
                if (_yAxisStepDoanhSoChart != value)
                {
                    _yAxisStepDoanhSoChart = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _yAxisStepColumnChart;
        public double YAxisStepColumnChart
        {
            get => _yAxisStepColumnChart;
            set
            {
                if (_yAxisStepColumnChart != value)
                {
                    _yAxisStepColumnChart = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _yAxisStepDebtColumnChart;
        public double YAxisStepDebtColumnChart
        {
            get => _yAxisStepDebtColumnChart;
            set
            {
                if (_yAxisStepDebtColumnChart != value)
                {
                    _yAxisStepDebtColumnChart = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _WidgetSoLuongDaiLyDeltaText = "0";
        public string WidgetSoLuongDaiLyDeltaText
        {
            get => _WidgetSoLuongDaiLyDeltaText;
            set
            {
                if (_WidgetSoLuongDaiLyDeltaText != value)
                {
                    _WidgetSoLuongDaiLyDeltaText = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _widgetSoLuongDaiLyIsUp = true;
        public bool WidgetSoLuongDaiLyIsUp
        {
            get => _widgetSoLuongDaiLyIsUp;
            set
            {
                if (_widgetSoLuongDaiLyIsUp != value)
                {
                    _widgetSoLuongDaiLyIsUp = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _widgetPhieuThuDeltaText = "0%";
        public string WidgetPhieuThuDeltaText
        {
            get => _widgetPhieuThuDeltaText;
            set
            {
                if (_widgetPhieuThuDeltaText != value)
                {
                    _widgetPhieuThuDeltaText = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _widgetPhieuThuIsUp = true;
        public bool WidgetPhieuThuIsUp
        {
            get => _widgetPhieuThuIsUp;
            set
            {
                if (_widgetPhieuThuIsUp != value)
                {
                    _widgetPhieuThuIsUp = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _widgetPhieuXuatDeltaText = "0%";
        public string WidgetPhieuXuatDeltaText
        {
            get => _widgetPhieuXuatDeltaText;
            set
            {
                if (_widgetPhieuXuatDeltaText != value)
                {
                    _widgetPhieuXuatDeltaText = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _widgetPhieuXuatIsUp = true;
        public bool WidgetPhieuXuatIsUp
        {
            get => _widgetPhieuXuatIsUp;
            set
            {
                if (_widgetPhieuXuatIsUp != value)
                {
                    _widgetPhieuXuatIsUp = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _WidgetTongGiaTriPhieuThu = "0 đ";
        public string WidgetTongGiaTriPhieuThu
        {
            get => _WidgetTongGiaTriPhieuThu;
            set
            {
                if (_WidgetTongGiaTriPhieuThu != value)
                {
                    _WidgetTongGiaTriPhieuThu = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _WidgetTongGiaTriPhieuXuat = "0 đ";
        public string WidgetTongGiaTriPhieuXuat
        {
            get => _WidgetTongGiaTriPhieuXuat;
            set
            {
                if (_WidgetTongGiaTriPhieuXuat != value)
                {
                    _WidgetTongGiaTriPhieuXuat = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _WidgetSoLuongDaiLy = "";
        public string WidgetSoLuongDaiLy
        {
            get => _WidgetSoLuongDaiLy;
            set
            {
                if (_WidgetSoLuongDaiLy != value)
                {
                    _WidgetSoLuongDaiLy = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _WidgetDoanhThuThang="";
        public string WidgetDoanhThuThang
        {
            get => _WidgetDoanhThuThang;
            set
            {
                if (_WidgetDoanhThuThang != value)
                {
                    _WidgetDoanhThuThang = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _revenueChartMonth = "";
        public string RevenueChartMonth
        {
            get => _revenueChartMonth;
            set
            {
                if (_revenueChartMonth != value)
                {
                    _revenueChartMonth = value;
                    OnPropertyChanged();
                    _=UpdateLineChart();
                    _ = UpdateWidgetSoLuongDaiLy();
                    _ = UpdateWidgetDoanhThuThang();
                    _ = UpdateWidgetTongGiaTriPhieuThu();
                    _ = UpdateWidgetTongGiaTriPhieuXuat();
                    _ = UpdateDoanhSoChart();
                }
            }
        }

        private int _revenueChartYear = 0;
        public int RevenueChartYear
        {
            get => _revenueChartYear;
            set
            {
                if (_revenueChartYear != value)
                {
                    _revenueChartYear = value;
                    OnPropertyChanged();
                    _ = UpdateLineChart();
                    _ = UpdateWidgetSoLuongDaiLy();
                    _ = UpdateWidgetDoanhThuThang();
                    _ = UpdateWidgetTongGiaTriPhieuThu();
                    _ = UpdateWidgetTongGiaTriPhieuXuat();
                    _ = UpdateDoanhSoChart();
                }
            }
        }

        private string _pieChartMonth = "";
        public string PieChartMonth
        {
            get => _pieChartMonth;
            set
            {
                if (_pieChartMonth != value)
                {
                    _pieChartMonth = value;
                    OnPropertyChanged();
                    _ = UpdatePieChart();
                    _ = UpdateQuanPieChart(); // TODO: create new pie chart month and remove this
                }
            }
        }

        private int _pieChartYear = 0;
        public int PieChartYear
        {
            get => _pieChartYear;
            set
            {
                if (_pieChartYear != value)
                {
                    _pieChartYear = value;
                    OnPropertyChanged();
                    _ = UpdatePieChart();
                    _ = UpdateQuanPieChart(); // TODO: create new pie chart year and remove this 
                }
            }
        }

        private string _topAgentChartMonth = "";
        public string TopAgentChartMonth
        {
            get => _topAgentChartMonth;
            set
            {
                if (_topAgentChartMonth != value)
                {
                    _topAgentChartMonth = value;
                    OnPropertyChanged();
                    _ = UpdateColumnChart();
                }
            }
        }

        private int _topAgentChartYear = 0;
        public int TopAgentChartYear
        {
            get => _topAgentChartYear;
            set
            {
                if (_topAgentChartYear != value)
                {
                    _topAgentChartYear = value;
                    OnPropertyChanged();
                    _ = UpdateColumnChart();
                }
            }
        }

        private string _debtChartMonth = "";
        public string DebtChartMonth
        {
            get => _debtChartMonth;
            set
            {
                if (_debtChartMonth != value)
                {
                    _debtChartMonth = value;
                    OnPropertyChanged();
                    _ = UpdateDebtColumnChart();
                }
            }
        }

        private int _debtChartYear = 0;
        public int DebtChartYear
        {
            get => _debtChartYear;
            set
            {
                if (_debtChartYear != value)
                {
                    _debtChartYear = value;
                    OnPropertyChanged();
                    _ = UpdateDebtColumnChart();
                }
            }
        }

        // Keep these for backward compatibility
        private string _selectedMonth = "";
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged();

                    // Update all chart-specific month properties to keep consistency
                    RevenueChartMonth = value;
                    PieChartMonth = value;
                    TopAgentChartMonth = value;
                    DebtChartMonth = value;
                }
            }
        }

        private int _selectedYear = 0;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();

                    // Update all chart-specific year properties to keep consistency
                    RevenueChartYear = value;
                    PieChartYear = value;
                    TopAgentChartYear = value;
                    DebtChartYear = value;
                }
            }
        }
        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}