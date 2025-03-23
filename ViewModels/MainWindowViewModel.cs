using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace QuanLyDaiLy.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _dailyService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, ChinhSuaDaiLyViewModel> _chinhSuaDaiLyFactory;

        public MainWindowViewModel(
            IDaiLyService dailyService,
            IServiceProvider serviceProvider,
            Func<int, ChinhSuaDaiLyViewModel> chinhSuaDaiLyFactory
        ) {
            _dailyService = dailyService;
            _ = LoadData();

            OpenHoSoDaiLyCommand = new RelayCommand(OpenHoSoDaiLyWindow);
            EditDaiLyCommand = new RelayCommand(OpenChinhSuaDaiLyWindow);
            DeleteDaiLyCommand = new RelayCommand(OpenDeleteDaiLyWindow);
            SearchDaiLyCommand = new RelayCommand(OpenSearchDaiLyWindow);
            _serviceProvider = serviceProvider;
            _chinhSuaDaiLyFactory = chinhSuaDaiLyFactory;
            _selectedDaiLy = null!;        
        }

        private ObservableCollection<DaiLy> _danhSachDaiLy = [];
        public ObservableCollection<DaiLy> DanhSachDaiLy
        {
            get => _danhSachDaiLy;
            set
            {
                _danhSachDaiLy = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadData()
        {
            var list = await _dailyService.GetAllDaiLy();
            DanhSachDaiLy = [.. list];
        }

        public ICommand OpenHoSoDaiLyCommand { get; }
        public ICommand EditDaiLyCommand { get; }
        public ICommand DeleteDaiLyCommand { get; }
        public ICommand SearchDaiLyCommand { get; }

        private void OpenHoSoDaiLyWindow()
        {
            var hoSoDaiLyWindow = _serviceProvider.GetRequiredService<HoSoDaiLyWinDow>();

            if (hoSoDaiLyWindow.DataContext is HoSoDaiLyViewModel viewModel)
            {
                viewModel.DataChanged += async (sender, e) => await LoadData();
            }

            hoSoDaiLyWindow.Show();
        }

        private void OpenDeleteDaiLyWindow()
        {
            var hoSoDaiLyWindow = _serviceProvider.GetRequiredService<HoSoDaiLyWinDow>();
            hoSoDaiLyWindow.Show();
        }

        private void OpenSearchDaiLyWindow()
        {
            var hoSoDaiLyWindow = _serviceProvider.GetRequiredService<HoSoDaiLyWinDow>();
            hoSoDaiLyWindow.Show();
        }

        private DaiLy _selectedDaiLy;
        public DaiLy SelectedDaiLy
        {
            get => _selectedDaiLy;
            set
            {
                _selectedDaiLy = value;
                OnPropertyChanged();
            }
        }

        private void OpenChinhSuaDaiLyWindow()
        {
            if (SelectedDaiLy == null)
            {
                MessageBox.Show("Vui lòng chọn đại lý để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewModel = _chinhSuaDaiLyFactory(SelectedDaiLy.MaDaiLy);
                viewModel.DataChanged += async (sender, e) => await LoadData();

                var window = new ChinhSuaDaiLyWindow(viewModel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}