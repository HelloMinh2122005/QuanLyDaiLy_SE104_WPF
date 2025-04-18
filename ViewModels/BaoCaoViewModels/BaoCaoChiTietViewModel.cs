using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views.BaoCaoViews;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public class BaoCaoChiTietViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;

        public BaoCaoChiTietViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            DoanhSoCommand = new RelayCommand(OpenDoanhSoWindow);
            CongNoCommand = new RelayCommand(OpenCongNoWindow);

            InitializeMonthYearOptions();
            InitializeDoanhSoData();
            InitializeCongNoData();
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

        private string _selectedDoanhSoMonth = "Tháng 4";
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

        private int _selectedDoanhSoYear = 2025;
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

        private string _selectedCongNoMonth = "Tháng 4";
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

        private int _selectedCongNoYear = 2025; 
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

        private void InitializeDoanhSoData()
        {
            var dailyNames = new[] {
                "Việt Tiến",
                "Hà Nội",
                "Minh Quang",
                "An Phát",
                "Thành Công",
                "Phú Đạt",
                "Hoàng Anh",
                "Kim Cương",
                "Đại Phát",
                "Tân Thanh"
            };

            var dailyIncomes = new[] {
                45000000,  // 45 million
                38500000,  // 38.5 million
                32000000,  // 32 million
                27500000,  // 27.5 million
                25000000,  // 25 million
                22500000,  // 22.5 million
                20000000,  // 20 million
                18000000,  // 18 million
                16500000,  // 16.5 million
                15000000   // 15 million
            };

            var dailyIncomesDouble = dailyIncomes.Select(x => (double)x).ToArray();

            DoanhSoLabels = dailyNames;
            DoanhSoSeries =
            [
                new ColumnSeries
                {
                    Title = "Doanh số",
                    Values = new ChartValues<double>(dailyIncomesDouble),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)), // Green
                    MaxColumnWidth = 50
                }
            ];
        }

        private void InitializeCongNoData()
        {
            var dailyNames = new[] {
                "Minh Quang",
                "Việt Tiến",
                "Tân Thành",
                "Hoàng Gia",
                "Đại Phát",
                "Thành Đạt",
                "Thái Dương",
                "Tiến Phát",
                "Kim Cương",
                "Phúc Lộc"
            };

            var random = new Random(DateTime.Now.Millisecond);
            var dailyDebts = new double[10];

            for (int i = 0; i < 10; i++)
            {
                dailyDebts[i] = 25000000 + random.Next(35000000);
            }

            // Sort by debt value (descending)
            var sortedData = dailyNames.Zip(dailyDebts, (name, debt) => new { Name = name, Debt = debt })
                .OrderByDescending(item => item.Debt)
                .ToArray();

            // Store names and debt values in the correct order
            CongNoLabels = sortedData.Select(item => item.Name).ToArray();
            var sortedDebts = sortedData.Select(item => item.Debt).ToArray();

            CongNoSeries =
            [
                new ColumnSeries
                {
                    Title = "Công nợ",
                    Values = new ChartValues<double>(sortedDebts),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)),
                    MaxColumnWidth = 50 
                }
            ];
        }

        private void UpdateDoanhSoData()
        {
            var random = new Random(SelectedDoanhSoMonth.GetHashCode() + SelectedDoanhSoYear);

            var newValues = new ChartValues<double>();
            for (int i = 0; i < DoanhSoLabels.Length; i++)
            {
                newValues.Add(15000000 + random.Next(35000000));
            }

            DoanhSoSeries[0].Values = newValues;
        }

        private void UpdateCongNoData()
        {
            var random = new Random(SelectedCongNoMonth.GetHashCode() + SelectedCongNoYear);

            var newValues = new ChartValues<double>();
            for (int i = 0; i < CongNoLabels.Length; i++)
            {
                newValues.Add(20000000 + random.Next(30000000));
            }

            CongNoSeries[0].Values = newValues;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
