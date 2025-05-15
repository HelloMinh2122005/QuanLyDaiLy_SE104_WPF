using LiveCharts.Wpf;
using LiveCharts;
using System.Windows.Media;
using QuanLyDaiLy.Views.BaoCaoViews;
using QuanLyDaiLy.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public partial class BaoCaoChiTietViewModel : ObservableObject
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

            WeakReferenceMessenger.Default.RegisterAll(this);

            InitializeMonthYearOptions();
            _ = InitializeAllData();
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
            _ = UpdateDoanhSoData();
        }

        [ObservableProperty]
        private int _selectedDoanhSoYear = DateTime.Now.Year;

        partial void OnSelectedDoanhSoYearChanged(int value)
        {
            _ = UpdateDoanhSoData();
        }

        [ObservableProperty]
        private string _selectedCongNoMonth = $"Tháng {DateTime.Now.Month}";

        partial void OnSelectedCongNoMonthChanged(string value)
        {
            _ = UpdateCongNoData();
        }

        [ObservableProperty]
        private int _selectedCongNoYear = DateTime.Now.Year;

        partial void OnSelectedCongNoYearChanged(int value)
        {
            _ = UpdateCongNoData();
        }

        [RelayCommand]
        private void DoanhSo()
        {
            try
            {
                var window = _serviceProvider.GetRequiredService<BaoCaoDoanhSoWindow>();
                int month = int.Parse(SelectedDoanhSoMonth.Replace("Tháng ", ""));
                int year = SelectedDoanhSoYear;
                WeakReferenceMessenger.Default.Send(new SelectedDateMessage(month, year));
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ báo cáo doanh số: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        [RelayCommand]
        private void CongNo()
        {
            try
            {
                var window = _serviceProvider.GetRequiredService<BaoCaoCongNoWindow>();
                int month = int.Parse(SelectedCongNoMonth.Replace("Tháng ", ""));
                int year = SelectedCongNoYear;
                WeakReferenceMessenger.Default.Send(new SelectedDateMessage(month, year));
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ báo cáo công nợ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private async Task InitializeAllData()
        {
            try
            {
                // Chạy song song các hàm khởi tạo
                await Task.WhenAll(
                    InitializeDoanhSoData(),
                    InitializeCongNoData()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi khởi tạo dữ liệu: {ex.Message}");
            }
        }

        public async Task InitializeDoanhSoData()
        {
            // Kiểm tra và lấy tháng từ chuỗi "Tháng x"
            if (string.IsNullOrEmpty(SelectedDoanhSoMonth) ||
                !int.TryParse(new string(SelectedDoanhSoMonth.Where(char.IsDigit).ToArray()), out int selectedMonth))
                return;

            int selectedYear = SelectedDoanhSoYear;

            try
            {
                // Khởi tạo các Task lấy dữ liệu đồng thời
                var allPhieuXuatsTask = _phieuXuatService.GetAllPhieuXuat();
                var allDaiLysTask = _daiLyService.GetAllDaiLy();

                // Chờ cả hai tác vụ hoàn thành
                var allPhieuXuats = await allPhieuXuatsTask;
                var allDaiLys = await allDaiLysTask;

                // Lọc phiếu xuất theo tháng và năm đã chọn
                var filteredPhieuXuats = allPhieuXuats
                    .Where(p => p.NgayLapPhieu.Month == selectedMonth && p.NgayLapPhieu.Year == selectedYear)
                    .GroupBy(p => p.MaDaiLy)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.TongTriGia));

                // Tạo danh sách đại lý với doanh số tương ứng
                var daiLyDoanhSoList = allDaiLys
                    .Select(d => new
                    {
                        d.TenDaiLy,
                        TongDoanhSo = filteredPhieuXuats.TryGetValue(d.MaDaiLy, out var value) ? value : 0
                    })
                    .OrderByDescending(d => d.TongDoanhSo)
                    .Take(10)
                    .ToList();

                // Cập nhật label và series cho biểu đồ doanh số
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
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý ngoại lệ tùy theo yêu cầu
                Console.WriteLine($"Lỗi khi khởi tạo dữ liệu doanh số: {ex.Message}");
            }
        }
        public async Task InitializeCongNoData()
        {
            try
            {
                // Lấy tháng từ chuỗi "Tháng x"
                if (!int.TryParse(new string(SelectedCongNoMonth.Where(char.IsDigit).ToArray()), out int selectedMonth))
                    return;

                int selectedYear = SelectedCongNoYear;

                // Lấy danh sách đại lý
                var daiLyList = await _daiLyService.GetAllDaiLy();

                // Trường hợp không có đại lý
                if (daiLyList == null || !daiLyList.Any())
                {
                    CongNoLabels = Array.Empty<string>();
                    CongNoSeries = new SeriesCollection();
                    return;
                }

                // Khởi tạo danh sách các tác vụ bất đồng bộ cho các đại lý
                var tasks = daiLyList.Select(async daiLy =>
                {
                    try
                    {
                        // Lấy dữ liệu phiếu thu và phiếu xuất cho đại lý
                        var phieuThu = await _phieuThuService.GetPhieuThuByDaiLyId(daiLy.MaDaiLy);
                        var phieuXuat = await _phieuXuatService.GetPhieuXuatByDaiLyId(daiLy.MaDaiLy);

                        var phieuThus = phieuThu
                            .Where(p => p.NgayThuTien.Month == selectedMonth && p.NgayThuTien.Year == selectedYear);
                        var phieuXuats = phieuXuat
                            .Where(p => p.NgayLapPhieu.Month == selectedMonth && p.NgayLapPhieu.Year == selectedYear);

                        double tongThu = phieuThus.Sum(p => p.SoTienThu);
                        double tongXuat = phieuXuats.Sum(p => p.TongTriGia);
                        double congNo = tongXuat - tongThu;

                        return (TenDaiLy: daiLy.TenDaiLy, CongNo: congNo);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi xử lý đại lý {daiLy.TenDaiLy}: {ex.Message}");
                        return (TenDaiLy: daiLy.TenDaiLy, CongNo: 0);
                    }
                }).ToList();

                // Chờ tất cả các tác vụ hoàn thành và lấy kết quả
                var daiLyCongNoList = await Task.WhenAll(tasks);

                // Sắp xếp và lọc ra top 10 công nợ lớn nhất
                var filteredData = daiLyCongNoList
                    .OrderByDescending(d => d.CongNo)
                    .Take(10)
                    .ToArray();

                // Trường hợp không có dữ liệu
                if (!filteredData.Any())
                {
                    CongNoLabels = Array.Empty<string>();
                    CongNoSeries = new SeriesCollection();
                    return;
                }

                // Cập nhật label và series cho biểu đồ công nợ
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
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi khởi tạo dữ liệu công nợ: {ex.Message}");
            }
        }



        private async Task UpdateDoanhSoData()
        {
            await InitializeDoanhSoData();
        }

        private async Task UpdateCongNoData()
        {
            await InitializeCongNoData();
        }

    }
}

