using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public class MatHangPageViewModel : INotifyPropertyChanged
    {
        private readonly IMatHangService _matHangService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, CapNhatMatHangWindowViewModel> _capNhatMatHangFactory;

        public MatHangPageViewModel(
            IMatHangService matHangService, 
            IServiceProvider serviceProvider,
            Func<int, CapNhatMatHangWindowViewModel> capNhatMatHangFactory
        ) {
            _matHangService = matHangService;
            _serviceProvider = serviceProvider;
            _capNhatMatHangFactory = capNhatMatHangFactory;

            SearchMatHangCommand = new RelayCommand(OpenSearchMatHangWindow);
            AddMatHangCommand = new RelayCommand(OpenAddMatHangWindow);
            EditMatHangCommand = new RelayCommand(OpenEditMatHangWindow);
            DeleteMatHangCommand = new RelayCommand(ExecuteDeleteMatHang);
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());

            _ = LoadDataAsync();
        }

        // Binding properties
        private ObservableCollection<MatHang> _danhSachMatHang = [];
        public ObservableCollection<MatHang> DanhSachMatHang
        {
            get => _danhSachMatHang;
            set
            {
                _danhSachMatHang = value;
                OnPropertyChanged();
            }
        }

        private MatHang _selectedMatHang = null!;
        public MatHang SelectedMatHang
        {
            get => _selectedMatHang;
            set
            {
                _selectedMatHang = value;
                OnPropertyChanged();
            }
        }

        private Visibility _previousButtonVisibility = Visibility.Collapsed;
        public Visibility PreviousButtonVisibility
        {
            get => _previousButtonVisibility;
            set
            {
                _previousButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _nextButtonVisibility = Visibility.Visible;
        public Visibility NextButtonVisibility
        {
            get => _nextButtonVisibility;
            set
            {
                _nextButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private Style _buttonStyleFirst;
        public Style ButtonStyleFirst
        {
            get => _buttonStyleFirst;
            set
            {
                _buttonStyleFirst = value;
                OnPropertyChanged();
            }
        }


        // Commands
        public ICommand SearchMatHangCommand { get; }
        public ICommand AddMatHangCommand { get; }
        public ICommand EditMatHangCommand { get; }
        public ICommand DeleteMatHangCommand { get; }
        public ICommand LoadDataCommand { get; }

        // Methods
        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _matHangService.GetAllMatHang();
                DanhSachMatHang = [.. list];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSearchMatHangWindow()
        {
            SelectedMatHang = null!;

            var traCuuMatHangWindow = _serviceProvider.GetRequiredService<TraCuuMatHangWindow>();

            if (traCuuMatHangWindow.DataContext is TraCuuMatHangWindowViewModel viewModel)
            {
                viewModel.SearchCompleted += (sender, searchResults) =>
                {
                    if (searchResults.Count > 0)
                    {
                        DanhSachMatHang = searchResults;
                    }
                };
            }

            traCuuMatHangWindow.Show();
        }

        private void OpenAddMatHangWindow()
        {
            try
            {
                var addMatHangWindow = _serviceProvider.GetRequiredService<ThemMatHangWindow>();
                if (addMatHangWindow.DataContext is ThemMatHangWindowViewModel viewModel)
                {
                    viewModel.DataChanged += async (sender, e) => await LoadDataAsync();
                }
                addMatHangWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SelectedMatHang = null!;
        }

        private void OpenEditMatHangWindow()
        {
            if (SelectedMatHang == null || string.IsNullOrEmpty(SelectedMatHang.TenMatHang))
            {
                MessageBox.Show("Vui lòng chọn mặt hàng để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewmodel = _capNhatMatHangFactory(SelectedMatHang.MaMatHang);
                viewmodel.DataChanged += async (sender, e) => await LoadDataAsync();

                var window = new CapNhatMatHangWindow(viewmodel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExecuteDeleteMatHang()
        {
            _ = DeleteMatHang();
        }

        private async Task DeleteMatHang()
        {
            if (SelectedMatHang == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa mặt hàng '{SelectedMatHang.TenMatHang}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _matHangService.DeleteMatHang(SelectedMatHang.MaMatHang);
                    MessageBox.Show("Đã xóa mặt hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async Task LoadDataExecute()
        {
            SelectedMatHang = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
