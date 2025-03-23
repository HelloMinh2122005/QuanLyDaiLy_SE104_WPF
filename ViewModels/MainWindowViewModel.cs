using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace QuanLyDaiLy.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _dailyService;
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(
            IDaiLyService dailyService, 
            IServiceProvider serviceProvider
        ) {
            _dailyService = dailyService;
            _ = LoadData();

            OpenHoSoDaiLyCommand = new RelayCommand(OpenHoSoDaiLyWindow);
            EditDaiLyCommand = new RelayCommand(OpenEditDaiLyWindow);
            DeleteDaiLyCommand = new RelayCommand(OpenDeleteDaiLyWindow);
            SearchDaiLyCommand = new RelayCommand(OpenSearchDaiLyWindow);
            _serviceProvider = serviceProvider;
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
            hoSoDaiLyWindow.Show();
        }

        private void OpenEditDaiLyWindow()
        {
            var hoSoDaiLyWindow = _serviceProvider.GetRequiredService<HoSoDaiLyWinDow>();
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}