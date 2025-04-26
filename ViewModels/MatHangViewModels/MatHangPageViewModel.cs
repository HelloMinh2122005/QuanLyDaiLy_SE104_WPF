using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public class MatHangPageViewModel : INotifyPropertyChanged
    {
        private readonly IMatHangService _matHangService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, CapNhatMatHangWindowViewModel> _capNhatMatHangFactory;

        // Pagination properties
        private int _currentPage = 0;
        private int _pageSize = 10;
        private int _totalItems = 0;
        private int _totalPages = 0;
        private ObservableCollection<int> _pageNumbers = [];

        public MatHangPageViewModel(
            IMatHangService matHangService,
            IServiceProvider serviceProvider,
            Func<int, CapNhatMatHangWindowViewModel> capNhatMatHangFactory
        )
        {
            _matHangService = matHangService;
            _serviceProvider = serviceProvider;
            _capNhatMatHangFactory = capNhatMatHangFactory;

            SearchMatHangCommand = new RelayCommand(OpenSearchMatHangWindow);
            AddMatHangCommand = new RelayCommand(OpenAddMatHangWindow);
            EditMatHangCommand = new RelayCommand(OpenEditMatHangWindow);
            DeleteMatHangCommand = new RelayCommand(ExecuteDeleteMatHang);
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());

            // Pagination commands
            NextPageCommand = new RelayCommand(async () => await NextPage(), () => CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(async () => await PreviousPage(), () => CanGoToPreviousPage);
            GoToPageCommand = new RelayCommand<int>(async (pageNumber) => await GoToPage(pageNumber));

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

        // Pagination properties
        public int CurrentPage
        {
            get => _currentPage + 1; // Display as 1-based index
            set
            {
                if (_currentPage != value - 1)
                {
                    _currentPage = value - 1; // Store as 0-based index
                    OnPropertyChanged();
                    _ = LoadDataAsync();
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    OnPropertyChanged();
                    _ = LoadDataAsync();
                }
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = value;
                OnPropertyChanged();
                CalculateTotalPages();
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set
            {
                _totalPages = value;
                OnPropertyChanged();
                UpdatePageNumbers();
            }
        }

        public bool CanGoToNextPage => CurrentPage < TotalPages;
        public bool CanGoToPreviousPage => CurrentPage > 1;

        public ObservableCollection<int> PageNumbers
        {
            get => _pageNumbers;
            set
            {
                _pageNumbers = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand SearchMatHangCommand { get; }
        public ICommand AddMatHangCommand { get; }
        public ICommand EditMatHangCommand { get; }
        public ICommand DeleteMatHangCommand { get; }
        public ICommand LoadDataCommand { get; }

        // Pagination commands
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand GoToPageCommand { get; }

        // Methods
        private async Task LoadDataAsync()
        {
            try
            {
                // Get count of all items for pagination
                var allItems = await _matHangService.GetAllMatHang();
                TotalItems = allItems.Count();

                // Load current page data
                var list = await _matHangService.GetMatHangPage(_currentPage * _pageSize, _pageSize);
                DanhSachMatHang = [.. list];

                // Notify command availability changes
                OnPropertyChanged(nameof(CanGoToNextPage));
                OnPropertyChanged(nameof(CanGoToPreviousPage));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateTotalPages()
        {
            TotalPages = (int)Math.Ceiling((double)_totalItems / _pageSize);
        }

        private void UpdatePageNumbers()
        {
            var numbers = new ObservableCollection<int>();

            // Simple logic: show first 3 pages, ellipsis, and last 3 pages if applicable
            if (TotalPages <= 6)
            {
                // If few pages, show all
                for (int i = 1; i <= TotalPages; i++)
                {
                    numbers.Add(i);
                }
            }
            else
            {
                // Show first 3
                for (int i = 1; i <= 3; i++)
                {
                    numbers.Add(i);
                }

                // Check if we need ellipsis or we're already close to the end
                if (CurrentPage > 4)
                {
                    numbers.Add(-1); // -1 represents ellipsis in UI
                }

                // Show pages around current
                int startPage = Math.Max(4, CurrentPage - 1);
                int endPage = Math.Min(TotalPages - 3, CurrentPage + 1);

                for (int i = startPage; i <= endPage; i++)
                {
                    if (!numbers.Contains(i) && i != -1)
                    {
                        numbers.Add(i);
                    }
                }

                // Add ellipsis before last pages if needed
                if (CurrentPage < TotalPages - 3)
                {
                    numbers.Add(-1);
                }

                // Show last 3 pages
                for (int i = Math.Max(TotalPages - 2, 4); i <= TotalPages; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        numbers.Add(i);
                    }
                }
            }

            PageNumbers = numbers;
        }

        private async Task NextPage()
        {
            if (CanGoToNextPage)
            {
                CurrentPage++;
                await LoadDataAsync();
            }
        }

        private async Task PreviousPage()
        {
            if (CanGoToPreviousPage)
            {
                CurrentPage--;
                await LoadDataAsync();
            }
        }

        private async Task GoToPage(int pageNumber)
        {
            if (pageNumber > 0 && pageNumber <= TotalPages && pageNumber != CurrentPage)
            {
                CurrentPage = pageNumber;
                await LoadDataAsync();
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

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
                return false;

            return _canExecute == null || _canExecute((T)(parameter ?? default(T)!));
        }

        public void Execute(object? parameter)
        {
            _execute((T)(parameter ?? default(T)!));
        }
    }
}