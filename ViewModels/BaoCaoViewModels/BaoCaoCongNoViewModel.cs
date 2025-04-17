using QuanLyDaiLy.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using QuanLyDaiLy.Views.BaoCaoViews;

namespace QuanLyDaiLy.ViewModels.BaoCaoViewModels
{
    public class BaoCaoCongNoViewModel : INotifyPropertyChanged
    {
        public BaoCaoCongNoViewModel()
        {
            CloseCommand = new RelayCommand(CloseWindow);

            InitializeMonthYearOptions();
        }

        public List<string> MonthOptions { get; set; } = [];
        public List<int> YearOptions { get; set; } = [];

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
                    // UpdateDoanhSoData();
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
                    // UpdateDoanhSoData();
                }
            }
        }

        public ICommand CloseCommand { get; }

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

            SelectedMonth = MonthOptions[currentMonth - 1];
            SelectedYear = currentYear;
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<BaoCaoCongNoWindow>().FirstOrDefault()?.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
