using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public class MatHangPageViewModel : ObservableObject
    {
        private readonly IMatHangService _matHangService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, CapNhatMatHangWindowViewModel> _capNhatMatHangFactory;

        // Total number of pages
        private int TotalPages = 0;

        // Number of page buttons to display
        private const int VisibleButtons = 5;

        public MatHangPageViewModel(
            IMatHangService matHangService,
            IServiceProvider serviceProvider,
            Func<int, CapNhatMatHangWindowViewModel> capNhatMatHangFactory
        )
        {
            _matHangService = matHangService;
            _serviceProvider = serviceProvider;
            _capNhatMatHangFactory = capNhatMatHangFactory;

            PageSelectionCommand = new RelayCommand<string>(SelectPage);

            SearchMatHangCommand = new RelayCommand(OpenSearchMatHangWindow);
            AddMatHangCommand = new RelayCommand(OpenAddMatHangWindow);
            EditMatHangCommand = new RelayCommand(OpenEditMatHangWindow);
            DeleteMatHangCommand = new RelayCommand(ExecuteDeleteMatHang);
            LoadDataCommand = new AsyncRelayCommand(LoadDataExecute);

            // Next/Previous page commands
            NextPageCommand = new RelayCommand(GoToNextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(GoToPreviousPage, CanGoToPreviousPage);

            _ = LoadDataAsync();
        }

        // Binding properties
        private string _currentPage = "1";
        public string CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                _ = UpdatePagination();

                // Update command can-execute state
                (NextPageCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (PreviousPageCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        // Page button content properties
        private string _buttonContentFirst = "1";
        public string ButtonContentFirst
        {
            get => _buttonContentFirst;
            set => SetProperty(ref _buttonContentFirst, value);
        }

        private string _buttonContentSecond = "2";
        public string ButtonContentSecond
        {
            get => _buttonContentSecond;
            set => SetProperty(ref _buttonContentSecond, value);
        }

        private string _buttonContentThird = "3";
        public string ButtonContentThird
        {
            get => _buttonContentThird;
            set => SetProperty(ref _buttonContentThird, value);
        }

        private string _buttonContentForth = "4";
        public string ButtonContentForth
        {
            get => _buttonContentForth;
            set => SetProperty(ref _buttonContentForth, value);
        }

        private string _buttonContentFith = "5";
        public string ButtonContentFith
        {
            get => _buttonContentFith;
            set => SetProperty(ref _buttonContentFith, value);
        }

        // Page button command parameter properties
        private string _buttonParamFirst = "1";
        public string ButtonParamFirst
        {
            get => _buttonParamFirst;
            set => SetProperty(ref _buttonParamFirst, value);
        }

        private string _buttonParamSecond = "2";
        public string ButtonParamSecond
        {
            get => _buttonParamSecond;
            set => SetProperty(ref _buttonParamSecond, value);
        }

        private string _buttonParamThird = "3";
        public string ButtonParamThird
        {
            get => _buttonParamThird;
            set => SetProperty(ref _buttonParamThird, value);
        }

        private string _buttonParamForth = "4";
        public string ButtonParamForth
        {
            get => _buttonParamForth;
            set => SetProperty(ref _buttonParamForth, value);
        }

        private string _buttonParamFith = "5";
        public string ButtonParamFith
        {
            get => _buttonParamFith;
            set => SetProperty(ref _buttonParamFith, value);
        }


        private ObservableCollection<MatHang> _danhSachMatHang = [];
        public ObservableCollection<MatHang> DanhSachMatHang
        {
            get => _danhSachMatHang;
            set => SetProperty(ref _danhSachMatHang, value);
        }

        private MatHang _selectedMatHang = null!;
        public MatHang SelectedMatHang
        {
            get => _selectedMatHang;
            set => SetProperty(ref _selectedMatHang, value);
        }

        private Style _buttonStyleFirst;
        public Style ButtonStyleFirst
        {
            get => _buttonStyleFirst;
            set => SetProperty(ref _buttonStyleFirst, value);
        }

        private Style _buttonStyleSecond;
        public Style ButtonStyleSecond
        {
            get => _buttonStyleSecond;
            set => SetProperty(ref _buttonStyleSecond, value);
        }

        private Style _buttonStyleThird;
        public Style ButtonStyleThird
        {
            get => _buttonStyleThird;
            set => SetProperty(ref _buttonStyleThird, value);
        }

        private Style _buttonStyleForth;
        public Style ButtonStyleForth
        {
            get => _buttonStyleForth;
            set => SetProperty(ref _buttonStyleForth, value);
        }

        private Style _buttonStyleFith;
        public Style ButtonStyleFith
        {
            get => _buttonStyleFith;
            set => SetProperty(ref _buttonStyleFith, value);
        }

        // Commands
        public ICommand SearchMatHangCommand { get; }
        public ICommand AddMatHangCommand { get; }
        public ICommand EditMatHangCommand { get; }
        public ICommand DeleteMatHangCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand PageSelectionCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        // Methods
        private async Task LoadDataAsync()
        {
            try
            {
                var list = await _matHangService.GetMatHangPage(0);
                DanhSachMatHang = [.. list];

                TotalPages = await _matHangService.GetTotalPages();
                if (TotalPages < 5)
                    ButtonStyleFith = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();
                if (TotalPages < 4)
                    ButtonStyleForth = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();
                if (TotalPages < 3)
                    ButtonStyleThird = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();
                if (TotalPages < 2)
                    ButtonStyleSecond = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();

                // Initialize pagination
                CurrentPage = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectPage(string pageNumber)
        {
            if (int.TryParse(pageNumber, out int page) && page >= 1 && page <= TotalPages)
            {
                CurrentPage = pageNumber;
            }
        }

        private bool CanGoToNextPage()
        {
            return int.TryParse(CurrentPage, out int current) && current < TotalPages;
        }

        private bool CanGoToPreviousPage()
        {
            return int.TryParse(CurrentPage, out int current) && current > 1;
        }

        private void GoToNextPage()
        {
            if (int.TryParse(CurrentPage, out int current) && current < TotalPages)
            {
                CurrentPage = (current + 1).ToString();
            }
        }

        private void GoToPreviousPage()
        {
            if (int.TryParse(CurrentPage, out int current) && current > 1)
            {
                CurrentPage = (current - 1).ToString();
            }
        }

        private async Task UpdatePagination()
        {
            if (!int.TryParse(CurrentPage, out int currentPage))
                return;

            // Cache application resources for better performance
            var unselectedStyle = Application.Current.Resources["PageButtonUnSelectedStyle"] as Style ?? new Style();
            var selectedStyle = Application.Current.Resources["PageButtonSelectedStyle"] as Style ?? new Style();
            var collapsedStyle = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();

            // Initialize all button styles to unselected
            var buttonStyles = new Style[VisibleButtons] {
                unselectedStyle, unselectedStyle, unselectedStyle,
                unselectedStyle, unselectedStyle
            };

            // Calculate page numbers for buttons
            string[] buttonContents = new string[VisibleButtons];
            string[] buttonParams = new string[VisibleButtons];

            // Calculate start page depending on total pages available
            int startPage = 1;

            if (TotalPages <= VisibleButtons)
            {
                // Simple case: when we have <= 5 pages, just number them 1 through 5
                for (int i = 0; i < VisibleButtons; i++)
                {
                    int pageNum = i + 1;
                    buttonContents[i] = buttonParams[i] = pageNum.ToString();

                    // Hide buttons beyond total pages
                    if (pageNum > TotalPages)
                        buttonStyles[i] = collapsedStyle;
                    // Set selected style for current page
                    else if (pageNum == currentPage)
                        buttonStyles[i] = selectedStyle;
                }
            }
            else // More than 5 pages
            {
                // Special cases for first two pages to show pages 1-5
                if (currentPage <= 2)
                {
                    startPage = 1;
                    for (int i = 0; i < VisibleButtons; i++)
                    {
                        buttonContents[i] = buttonParams[i] = (i + 1).ToString();
                    }

                    // Set the selected style for current page (1 or 2)
                    buttonStyles[currentPage - 1] = selectedStyle;
                }
                // Special cases for last few pages to show the last 5 pages
                else if (currentPage >= TotalPages - 2)
                {
                    startPage = TotalPages - 4;
                    for (int i = 0; i < VisibleButtons; i++)
                    {
                        int pageNum = startPage + i;
                        buttonContents[i] = buttonParams[i] = pageNum.ToString();
                    }

                    // Set selected style
                    int selectedIndex = currentPage - startPage;
                    buttonStyles[selectedIndex] = selectedStyle;
                }
                // Default sliding window: current page is in the middle
                else
                {
                    startPage = currentPage - 1;
                    for (int i = 0; i < VisibleButtons; i++)
                    {
                        int pageNum = startPage + i;
                        buttonContents[i] = buttonParams[i] = pageNum.ToString();
                    }

                    // Current page is always the second button in this case
                    buttonStyles[1] = selectedStyle;
                }
            }

            // Apply calculated values to properties
            ButtonContentFirst = buttonContents[0];
            ButtonParamFirst = buttonParams[0];
            ButtonStyleFirst = buttonStyles[0];

            ButtonContentSecond = buttonContents[1];
            ButtonParamSecond = buttonParams[1];
            ButtonStyleSecond = buttonStyles[1];

            ButtonContentThird = buttonContents[2];
            ButtonParamThird = buttonParams[2];
            ButtonStyleThird = buttonStyles[2];

            ButtonContentForth = buttonContents[3];
            ButtonParamForth = buttonParams[3];
            ButtonStyleForth = buttonStyles[3];

            ButtonContentFith = buttonContents[4];
            ButtonParamFith = buttonParams[4];
            ButtonStyleFith = buttonStyles[4];

            // Update the data - fetch only once
            var items = await _matHangService.GetMatHangPage(currentPage - 1);
            DanhSachMatHang.Clear();
            DanhSachMatHang = [.. items];
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
    }
}
