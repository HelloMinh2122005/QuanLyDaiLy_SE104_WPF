using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using QuanLyDaiLy.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using QuanLyDaiLy.Models;

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


            InitializeMonthYearOptions();
            _ = InitializeLineChart();
            _ = InitializePieChart();
            _ = InitializeColumnChart();
            _ = InitializeDebtColumnChart();
            _ = UpdateWidgetSoLuongDaiLy();
            _ = UpdateWidgetDoanhThuThang();
            _ = UpdateWidgetTongGiaTriPhieuThu();
            _ = UpdateWidgetTongGiaTriPhieuXuat();
        }

        private async Task InitializeLineChart()
        {
            // 1. Lấy tháng/năm hiện tại và năm trước
            int currentYear = _revenueChartYear;
            int lastYear = currentYear - 1;


            // Vietnamese month names
            MonthLabels = new[]
            {
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            };

            // Currency formatter for Vietnamese Dong
            CurrencyFormatter = value => value.ToString("N0") + " đ";

            // 2. Lấy tất cả các phiếu thu
            var allPhieuThu = await _phieuThuService.GetPhieuThuByCurrentYearAndLastYear(currentYear, lastYear);

            // 3. Tính tổng SoTienThu theo tháng của năm hiện tại và năm trước
            var thisYearData = new double[12];  // Lưu tổng SoTienThu của từng tháng trong năm hiện tại
            var lastYearData = new double[12];  // Lưu tổng SoTienThu của từng tháng trong năm trước

            foreach (var phieuThu in allPhieuThu)
            {
                int month = phieuThu.NgayThuTien.Month;
                int year = phieuThu.NgayThuTien.Year;

                // Tính tổng SoTienThu cho năm hiện tại
                if (year == currentYear)
                {
                    thisYearData[month - 1] += phieuThu.SoTienThu;
                }
                // Tính tổng SoTienThu cho năm trước
                if (year == lastYear)
                {
                    lastYearData[month - 1] += phieuThu.SoTienThu;
                }
            }


            // Nếu năm hiện tại không có dữ liệu cho một số tháng, gán giá trị 0 cho các tháng đó
            for (int i = 0; i < 12; i++)
            {
                if (thisYearData[i] == 0)
                {
                    thisYearData[i] = 0;  // Đảm bảo tháng không có dữ liệu sẽ có giá trị 0
                }
            }

            // 6. Cập nhật dữ liệu cho biểu đồ
            SpendingSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = currentYear.ToString(),
                    Values = new ChartValues<double>(thisYearData),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 12,  // Kích thước điểm
                    LineSmoothness = 0,      // Đường thẳng cho năm hiện tại
                    Stroke = System.Windows.Media.Brushes.Gray,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,  // Tắt nhãn dữ liệu trên đường thẳng
                    LabelPoint = point => point.Y.ToString("N0") + " đ",  // Định dạng tooltip
                    StrokeThickness = 3  // Độ dày đường thẳng
                },
                new LineSeries
                {
                    Title = lastYear.ToString(),
                    Values = new ChartValues<double>(lastYearData),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 12,  // Kích thước điểm
                    LineSmoothness = 1,      // Đường cong cho năm trước
                    Stroke = System.Windows.Media.Brushes.DodgerBlue,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,  // Tắt nhãn dữ liệu trên đường thẳng
                    LabelPoint = point => point.Y.ToString("N0") + " đ",  // Định dạng tooltip
                    StrokeThickness = 2  // Độ dày đường thẳng
                }
            };

            // Cập nhật UI
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



            var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
            var daiLyCounts = await _daiLyService.GetCountsGroupedByLoaiAsync(selectedMonth, selectedYear); // lấy count


            // Đếm số lượng đại lý theo loại

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
                    Fill = RandomColorBrush(loai.MaLoaiDaiLy) // Hoặc bạn dùng theo màu riêng
                });

                OnPropertyChanged(nameof(DaiLyDistributionSeries));

            }

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
                .OrderByDescending(d => d.TienNo)
                .Take(10)
                .ToList();

            // 7. Cập nhật nhãn và giá trị cho biểu đồ
            var debtNames = congNoTheoDaiLy.Select(d => d.TenDaiLy).ToList();
            var debtValues = congNoTheoDaiLy.Select(d => d.TienNo).ToList();

            TopDebtDaiLyLabels = debtNames.ToArray();
            OnPropertyChanged(nameof(TopDebtDaiLyLabels));

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

        private void InitializeMonthYearOptions()
        {
            // Get current date info
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            // Setup month options (Vietnamese)
            MonthOptions =
            [
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            ];

            // Setup year options (current year and 4 previous years)
            YearOptions = [];
            for (int i = currentYear - 4; i <= currentYear; i++)
            {
                YearOptions.Add(i);
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


        #endregion


        #region hàm update
        private async Task UpdateLineChart()
        {
            // 1. Lấy tháng/năm hiện tại và năm trước
            int currentYear = _revenueChartYear;
            int lastYear = currentYear - 1;

            // 2. Lấy tất cả các phiếu thu mới (phiếu thu có thể đã được thay đổi)
            var allPhieuThu = await _phieuThuService.GetPhieuThuByCurrentYearAndLastYear(currentYear, lastYear);

            // 3. Tính tổng SoTienThu theo tháng của năm hiện tại và năm trước
            var thisYearData = new double[12];  // Lưu tổng SoTienThu của từng tháng trong năm hiện tại
            var lastYearData = new double[12];  // Lưu tổng SoTienThu của từng tháng trong năm trước

            foreach (var phieuThu in allPhieuThu)
            {
                int month = phieuThu.NgayThuTien.Month;
                int year = phieuThu.NgayThuTien.Year;

                // Tính tổng SoTienThu cho năm hiện tại
                if (year == currentYear)
                {
                    thisYearData[month - 1] += phieuThu.SoTienThu;
                }
                // Tính tổng SoTienThu cho năm trước
                if (year == lastYear)
                {
                    lastYearData[month - 1] += phieuThu.SoTienThu;
                }
            }

            // 5. Nếu năm hiện tại thiếu dữ liệu cho các tháng, gán giá trị 0 cho các tháng đó
            for (int i = 0; i < 12; i++)
            {
                if (thisYearData[i] == 0)
                {
                    thisYearData[i] = 0;  // Đảm bảo tháng không có dữ liệu sẽ có giá trị 0
                }
            }

            // 6. Cập nhật dữ liệu cho biểu đồ
            SpendingSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = currentYear.ToString(),
                    Values = new ChartValues<double>(thisYearData),
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 12,  // Kích thước điểm
                    LineSmoothness = 0,      // Đường thẳng cho năm hiện tại
                    Stroke = System.Windows.Media.Brushes.Gray,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,  // Tắt nhãn dữ liệu trên đường thẳng
                    LabelPoint = point => point.Y.ToString("N0") + " đ",  // Định dạng tooltip
                    StrokeThickness = 3  // Độ dày đường thẳng
                },
                new LineSeries
                {
                    Title = lastYear.ToString(),
                    Values = new ChartValues<double>(lastYearData),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 12,  // Kích thước điểm
                    LineSmoothness = 1,      // Đường cong cho năm trước
                    Stroke = System.Windows.Media.Brushes.DodgerBlue,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,  // Tắt nhãn dữ liệu trên đường thẳng
                    LabelPoint = point => point.Y.ToString("N0") + " đ",  // Định dạng tooltip
                    StrokeThickness = 2  // Độ dày đường thẳng
                }
            };

            // 7. Cập nhật UI
            OnPropertyChanged(nameof(SpendingSeries));
        }

        private async Task UpdatePieChart()
        {
            // Lấy tháng/năm đang chọn
            int selectedMonth = GetSelectedMonthNumber(PieChartMonth); // ví dụ: "Tháng 4" => 4
            int selectedYear = PieChartYear;

            // Lấy danh sách loại đại lý và số lượng từng loại trong tháng/năm
            var listLoaiDaiLy = await _loaiDaiLyService.GetAllLoaiDaiLy();
            var daiLyCounts = await _daiLyService.GetCountsGroupedByLoaiAsync(selectedMonth, selectedYear);

            // Xoá và cập nhật lại dữ liệu cho biểu đồ
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
                    Fill = RandomColorBrush(loai.MaLoaiDaiLy) // Hoặc màu cố định
                });
            }

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
                .OrderByDescending(d => d.TienNo)
                .Take(10)
                .ToList();

            // 7. Cập nhật nhãn và giá trị cho biểu đồ
            var debtNames = congNoTheoDaiLy.Select(d => d.TenDaiLy).ToList();
            var debtValues = congNoTheoDaiLy.Select(d => d.TienNo).ToList();

            TopDebtDaiLyLabels = debtNames.ToArray();
            OnPropertyChanged(nameof(TopDebtDaiLyLabels));

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

        #endregion

        #region Binding Properties

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