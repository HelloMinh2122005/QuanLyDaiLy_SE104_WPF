﻿using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

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

        public DashboardPageViewModel()
        {
            InitializeMonthYearOptions();
            InitializeChart();
            InitializePieChart();
            InitializeColumnChart();
            InitializeDebtColumnChart();
        }

        private void InitializeChart()
        {
            // Vietnamese month names
            MonthLabels = [
                "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4",
                "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8",
                "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
            ];

            // Currency formatter for Vietnamese Dong
            CurrencyFormatter = value => value.ToString("N0") + " đ";

            // Sample data - replace with your actual data
            var thisMonthData = new ChartValues<double> {
                10000000, 15000000, 12000000, 18000000,
                22000000, 19000000, 25000000, 23000000,
                20000000, 27000000, 30000000, 32000000
            };

            var lastMonthData = new ChartValues<double> {
                8000000, 12000000, 10000000, 15000000,
                18000000, 16000000, 20000000, 19000000,
                17000000, 22000000, 25000000, 28000000
            };

            SpendingSeries =
            [
                new LineSeries
                {
                    Title = "2025",
                    Values = thisMonthData,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 12,             // Increased point size
                    LineSmoothness = 0,                 // Straight lines for current month (Gray)
                    Stroke = System.Windows.Media.Brushes.Gray,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,                 // Disable data labels on the line
                    LabelPoint = point => point.Y.ToString("N0") + " đ", // Format for tooltip
                    StrokeThickness = 3                 // Make the line thicker
                },
                new LineSeries
                {
                    Title = "2024",
                    Values = lastMonthData,
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 12,             // Increased point size
                    LineSmoothness = 1,                 // Curved lines with breaks for last month (Ocean)
                    Stroke = System.Windows.Media.Brushes.DodgerBlue,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,                 // Disable data labels on the line
                    LabelPoint = point => point.Y.ToString("N0") + " đ", // Format for tooltip
                    StrokeThickness = 2                 // Make the line thicker
                }
            ];
        }

        private void InitializePieChart()
        {
            // Specific counts for each LoaiDaiLy
            int loai1Count = 350;
            int loai2Count = 400;
            int loai3Count = 161;
            int loai4Count = 124;

            // Dummy data for pie chart with specific counts
            DaiLyDistributionSeries =
            [
                new PieSeries
                {
                    Title = $"Loại đại lý 1 ({loai1Count})",
                    Values = new ChartValues<double> { loai1Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(33, 150, 243)) // Blue
                },
                new PieSeries
                {
                    Title = $"Loại đại lý 2 ({loai2Count})",
                    Values = new ChartValues<double> { loai2Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)) // Green
                },
                new PieSeries
                {
                    Title = $"Loại đại lý 3 ({loai3Count})",
                    Values = new ChartValues<double> { loai3Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(255, 152, 0)) // Orange
                },
                new PieSeries
                {
                    Title = $"Loại đại lý 4 ({loai4Count})",
                    Values = new ChartValues<double> { loai4Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)) // Pink
                }
            ];
        }

        private void InitializeColumnChart()
        {
            // This would normally be populated from your database
            var dailyNames = new[] {
                "Việt Tiến",
                "Hà Nội",
                "Minh Quang",
                "An Phát",
                "Thành Công"
            };

            var dailyIncomes = new[] {
                45000000,  // 45 million
                38500000,  // 38.5 million
                32000000,  // 32 million
                27500000,  // 27.5 million
                25000000   // 25 million
            };

            var dailyIncomesDouble = dailyIncomes.Select(x => (double)x).ToArray();

            // Store names for X-axis labels
            TopDaiLyLabels = dailyNames;

            // Create a column series for the chart
            TopDaiLySeries =
            [
                new ColumnSeries
                {
                    Title = "Doanh số",
                    Values = new ChartValues<double>(dailyIncomesDouble),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)), // Green
                    MaxColumnWidth = 70
                }
            ];
        }

        private void InitializeDebtColumnChart()
        {
            // This would normally be populated from your database
            // Dummy data for top 10 DaiLy with highest Tiền nợ
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

            // Generate random debt values ranging from 25M to 60M
            for (int i = 0; i < 10; i++)
            {
                // Generate debt between 25,000,000 and 60,000,000
                dailyDebts[i] = 25000000 + random.Next(35000000);
            }

            // Sort by debt value (descending)
            var sortedData = dailyNames.Zip(dailyDebts, (name, debt) => new { Name = name, Debt = debt })
                .OrderByDescending(item => item.Debt)
                .ToArray();

            // Store names and debt values in the correct order
            TopDebtDaiLyLabels = sortedData.Select(item => item.Name).ToArray();
            var sortedDebts = sortedData.Select(item => item.Debt).ToArray();

            // Create a column series for the chart
            TopDebtDaiLySeries =
            [
                new ColumnSeries
                {
                    Title = "Công nợ",
                    Values = new ChartValues<double>(sortedDebts),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)), // Pink
                    MaxColumnWidth = 50
                }
            ];
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

        #region Chart-specific update methods

        private void UpdateLineChart()
        {
            // Create a seed value based on month and year for consistent "random" data
            var seed = RevenueChartMonth.GetHashCode() + RevenueChartYear;
            var random = new Random(seed);

            // Create new data for the current year
            var thisYearData = new ChartValues<double>();
            for (int i = 0; i < 12; i++)
            {
                // Generate a value between 10M and 40M
                thisYearData.Add(10000000 + random.Next(30000000));
            }

            // Create new data for the previous year
            var lastYearData = new ChartValues<double>();
            for (int i = 0; i < 12; i++)
            {
                // Generate a value between 8M and 30M
                lastYearData.Add(8000000 + random.Next(22000000));
            }

            // Create completely new series with the updated data
            SpendingSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Title = RevenueChartYear.ToString(),
                    Values = thisYearData,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 12,
                    LineSmoothness = 0,
                    Stroke = System.Windows.Media.Brushes.Gray,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    StrokeThickness = 3
                },
                new LineSeries
                {
                    Title = (RevenueChartYear - 1).ToString(),
                    Values = lastYearData,
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 12,
                    LineSmoothness = 1,
                    Stroke = System.Windows.Media.Brushes.DodgerBlue,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    DataLabels = false,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    StrokeThickness = 2
                }
            };

            OnPropertyChanged(nameof(SpendingSeries));
        }

        private void UpdatePieChart()
        {
            // Generate random counts based on month and year
            var seed = PieChartMonth.GetHashCode() + PieChartYear;
            var random = new Random(seed);

            int loai1Count = 300 + random.Next(200);  // 300-500
            int loai2Count = 350 + random.Next(150);  // 350-500
            int loai3Count = 150 + random.Next(100);  // 150-250
            int loai4Count = 100 + random.Next(100);  // 100-200

            // Create completely new series with the updated data
            DaiLyDistributionSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = $"Loại đại lý 1 ({loai1Count})",
                    Values = new ChartValues<double> { loai1Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(33, 150, 243)) // Blue
                },
                new PieSeries
                {
                    Title = $"Loại đại lý 2 ({loai2Count})",
                    Values = new ChartValues<double> { loai2Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)) // Green
                },
                new PieSeries
                {
                    Title = $"Loại đại lý 3 ({loai3Count})",
                    Values = new ChartValues<double> { loai3Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(255, 152, 0)) // Orange
                },
                new PieSeries
                {
                    Title = $"Loại đại lý 4 ({loai4Count})",
                    Values = new ChartValues<double> { loai4Count },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Participation:P1}",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)) // Pink
                }
            };

            OnPropertyChanged(nameof(DaiLyDistributionSeries));
        }

        private void UpdateColumnChart()
        {
            var seed = TopAgentChartMonth.GetHashCode() + TopAgentChartYear;
            var random = new Random(seed);

            // Get the current labels
            var dailyNames = TopDaiLyLabels;

            // Create new income data
            var newValues = new double[dailyNames.Length];
            for (int i = 0; i < dailyNames.Length; i++)
            {
                // Generate values between 20M and 55M
                newValues[i] = 20000000 + random.Next(35000000);
            }

            // Sort the data to show highest first
            var sortedData = dailyNames.Zip(newValues, (name, income) => new { Name = name, Income = income })
                .OrderByDescending(item => item.Income)
                .ToArray();

            // Update the labels
            TopDaiLyLabels = sortedData.Select(item => item.Name).ToArray();
            OnPropertyChanged(nameof(TopDaiLyLabels));

            // Create a completely new series
            TopDaiLySeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Doanh số",
                    Values = new ChartValues<double>(sortedData.Select(item => item.Income)),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(76, 175, 80)), // Green
                    MaxColumnWidth = 70
                }
            };

            OnPropertyChanged(nameof(TopDaiLySeries));
        }

        private void UpdateDebtColumnChart()
        {
            var seed = DebtChartMonth.GetHashCode() + DebtChartYear;
            var random = new Random(seed);

            // Get the current labels
            var dailyNames = TopDebtDaiLyLabels;

            // Create new debt data for each agent
            var newValues = new double[dailyNames.Length];
            for (int i = 0; i < dailyNames.Length; i++)
            {
                // Generate debt between 25M and 60M
                newValues[i] = 25000000 + random.Next(35000000);
            }

            // Sort by debt value (descending)
            var sortedData = dailyNames.Zip(newValues, (name, debt) => new { Name = name, Debt = debt })
                .OrderByDescending(item => item.Debt)
                .ToArray();

            // Update the labels
            TopDebtDaiLyLabels = sortedData.Select(item => item.Name).ToArray();
            OnPropertyChanged(nameof(TopDebtDaiLyLabels));

            // Create a completely new series
            TopDebtDaiLySeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Công nợ",
                    Values = new ChartValues<double>(sortedData.Select(item => item.Debt)),
                    DataLabels = true,
                    LabelPoint = point => point.Y.ToString("N0") + " đ",
                    Fill = new SolidColorBrush(Color.FromRgb(233, 30, 99)), // Pink
                    MaxColumnWidth = 50
                }
            };

            OnPropertyChanged(nameof(TopDebtDaiLySeries));
        }


        private string _revenueChartMonth = "Tháng 4";
        public string RevenueChartMonth
        {
            get => _revenueChartMonth;
            set
            {
                if (_revenueChartMonth != value)
                {
                    _revenueChartMonth = value;
                    OnPropertyChanged();
                    UpdateLineChart();
                }
            }
        }

        private int _revenueChartYear = 2025;
        public int RevenueChartYear
        {
            get => _revenueChartYear;
            set
            {
                if (_revenueChartYear != value)
                {
                    _revenueChartYear = value;
                    OnPropertyChanged();
                    UpdateLineChart();
                }
            }
        }

        private string _pieChartMonth = "Tháng 4";
        public string PieChartMonth
        {
            get => _pieChartMonth;
            set
            {
                if (_pieChartMonth != value)
                {
                    _pieChartMonth = value;
                    OnPropertyChanged();
                    UpdatePieChart();
                }
            }
        }

        private int _pieChartYear = 2025;
        public int PieChartYear
        {
            get => _pieChartYear;
            set
            {
                if (_pieChartYear != value)
                {
                    _pieChartYear = value;
                    OnPropertyChanged();
                    UpdatePieChart();
                }
            }
        }

        private string _topAgentChartMonth = "Tháng 4";
        public string TopAgentChartMonth
        {
            get => _topAgentChartMonth;
            set
            {
                if (_topAgentChartMonth != value)
                {
                    _topAgentChartMonth = value;
                    OnPropertyChanged();
                    UpdateColumnChart();
                }
            }
        }

        private int _topAgentChartYear = 2025;
        public int TopAgentChartYear
        {
            get => _topAgentChartYear;
            set
            {
                if (_topAgentChartYear != value)
                {
                    _topAgentChartYear = value;
                    OnPropertyChanged();
                    UpdateColumnChart();
                }
            }
        }

        private string _debtChartMonth = "Tháng 4";
        public string DebtChartMonth
        {
            get => _debtChartMonth;
            set
            {
                if (_debtChartMonth != value)
                {
                    _debtChartMonth = value;
                    OnPropertyChanged();
                    UpdateDebtColumnChart();
                }
            }
        }

        private int _debtChartYear = 2025;
        public int DebtChartYear
        {
            get => _debtChartYear;
            set
            {
                if (_debtChartYear != value)
                {
                    _debtChartYear = value;
                    OnPropertyChanged();
                    UpdateDebtColumnChart();
                }
            }
        }

        #endregion

        // Keep these for backward compatibility
        private string _selectedMonth = "Tháng 4";
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

        private int _selectedYear = 2025;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
