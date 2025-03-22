using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views;
using System.Windows;
using System.Windows.Input;

namespace QuanLyDaiLy.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IDaiLyService _dailyService;

        public MainWindowViewModel(IDaiLyService dailyService)
        {
            _dailyService = dailyService;
            _ = LoadData();

            OpenHoSoDaiLyCommand = new RelayCommand(OpenHoSoDaiLyWindow);
            EditDaiLyCommand = new RelayCommand(OpenEditDaiLyWindow);
            DeleteDaiLyCommand = new RelayCommand(OpenDeleteDaiLyWindow);
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

        private void OpenHoSoDaiLyWindow()
        {
            var hoSoDaiLyWindow = new HoSoDaiLyWinDow
            {
                Owner = Application.Current.MainWindow
            };
            hoSoDaiLyWindow.ShowDialog();
        }

        private void OpenEditDaiLyWindow()
        {
            var hoSoDaiLyWindow = new HoSoDaiLyWinDow
            {
                Owner = Application.Current.MainWindow
            };
            hoSoDaiLyWindow.ShowDialog();
        }

        private void OpenDeleteDaiLyWindow()
        {
            var hoSoDaiLyWindow = new HoSoDaiLyWinDow
            {
                Owner = Application.Current.MainWindow
            };
            hoSoDaiLyWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}