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

        private int TotalPages = 0;
        private const int VisibleButtons = 5;
        private const int ItemsPerPage = 20;

        private record struct PaginationButton(string Content, string Parameter, Style Style);

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
            SearchMatHangCommand = new AsyncRelayCommand(OpenSearchMatHangWindow);
            AddMatHangCommand = new RelayCommand(OpenAddMatHangWindow);
            EditMatHangCommand = new RelayCommand(OpenEditMatHangWindow);
            DeleteMatHangCommand = new RelayCommand(ExecuteDeleteMatHang);
            LoadDataCommand = new AsyncRelayCommand(LoadDataExecute);
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

        private ObservableCollection<MatHang> _filteredMatHangs = [];
        public ObservableCollection<MatHang> FilteredMatHangs
        {
            get => _filteredMatHangs;
            set => SetProperty(ref _filteredMatHangs, value);
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
                FilteredMatHangs.Clear();
                var list = await _matHangService.GetMatHangPage(0);
                DanhSachMatHang = [.. list];

                TotalPages = await _matHangService.GetTotalPages();
                UpdateButtonVisibility();

                // Initialize pagination
                CurrentPage = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButtonVisibility()
        {
            var collapsedStyle = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();

            if (TotalPages < 5)
                ButtonStyleFith = collapsedStyle;
            if (TotalPages < 4)
                ButtonStyleForth = collapsedStyle;
            if (TotalPages < 3)
                ButtonStyleThird = collapsedStyle;
            if (TotalPages < 2)
                ButtonStyleSecond = collapsedStyle;
        }

        private void SelectPage(string pageNumber)
        {
            if (int.TryParse(pageNumber, out int page) && page >= 1 && page <= TotalPages)
            {
                CurrentPage = pageNumber;
            }
        }

        private bool CanGoToNextPage() => int.TryParse(CurrentPage, out int current) && current < TotalPages;

        private bool CanGoToPreviousPage() => int.TryParse(CurrentPage, out int current) && current > 1;

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
            PaginationButton[] buttons = CalculatePaginationButtons(currentPage);
            ApplyPaginationButtonSettings(buttons);
            await LoadPageData(currentPage);
        }

        private PaginationButton[] CalculatePaginationButtons(int currentPage)
        {
            var unselectedStyle = Application.Current.Resources["PageButtonUnSelectedStyle"] as Style ?? new Style();
            var selectedStyle = Application.Current.Resources["PageButtonSelectedStyle"] as Style ?? new Style();
            var collapsedStyle = Application.Current.Resources["CollapsedButton"] as Style ?? new Style();

            var buttons = new PaginationButton[VisibleButtons];
            int startPage = 1;

            if (TotalPages <= VisibleButtons)
            {
                // Simple case: not enough pages to need complex pagination
                for (int i = 0; i < VisibleButtons; i++)
                {
                    int pageNum = i + 1;
                    string pageText = pageNum.ToString();

                    Style buttonStyle = pageNum > TotalPages
                        ? collapsedStyle
                        : (pageNum == currentPage ? selectedStyle : unselectedStyle);

                    buttons[i] = new PaginationButton(pageText, pageText, buttonStyle);
                }
            }
            else if (currentPage <= 2)
            {
                // Near the start
                for (int i = 0; i < VisibleButtons; i++)
                {
                    string pageText = (i + 1).ToString();
                    Style buttonStyle = (i + 1) == currentPage ? selectedStyle : unselectedStyle;
                    buttons[i] = new PaginationButton(pageText, pageText, buttonStyle);
                }
            }
            else if (currentPage >= TotalPages - 2)
            {
                // Near the end
                startPage = TotalPages - 4;
                for (int i = 0; i < VisibleButtons; i++)
                {
                    int pageNum = startPage + i;
                    string pageText = pageNum.ToString();
                    Style buttonStyle = pageNum == currentPage ? selectedStyle : unselectedStyle;
                    buttons[i] = new PaginationButton(pageText, pageText, buttonStyle);
                }
            }
            else
            {
                // In the middle
                startPage = currentPage - 2;
                for (int i = 0; i < VisibleButtons; i++)
                {
                    int pageNum = startPage + i;
                    string pageText = pageNum.ToString();
                    Style buttonStyle = pageNum == currentPage ? selectedStyle : unselectedStyle;
                    buttons[i] = new PaginationButton(pageText, pageText, buttonStyle);
                }
            }

            return buttons;
        }

        private void ApplyPaginationButtonSettings(PaginationButton[] buttons)
        {
            // First button
            ButtonContentFirst = buttons[0].Content;
            ButtonParamFirst = buttons[0].Parameter;
            ButtonStyleFirst = buttons[0].Style;

            // Second button
            ButtonContentSecond = buttons[1].Content;
            ButtonParamSecond = buttons[1].Parameter;
            ButtonStyleSecond = buttons[1].Style;

            // Third button
            ButtonContentThird = buttons[2].Content;
            ButtonParamThird = buttons[2].Parameter;
            ButtonStyleThird = buttons[2].Style;

            // Fourth button
            ButtonContentForth = buttons[3].Content;
            ButtonParamForth = buttons[3].Parameter;
            ButtonStyleForth = buttons[3].Style;

            // Fifth button
            ButtonContentFith = buttons[4].Content;
            ButtonParamFith = buttons[4].Parameter;
            ButtonStyleFith = buttons[4].Style;
        }

        private async Task LoadPageData(int currentPage)
        {
            if (FilteredMatHangs.Count == 0)
            {
                // Load from service when no filter is applied
                var items = await _matHangService.GetMatHangPage(currentPage - 1);
                DanhSachMatHang = [.. items];
            }
            else
            {
                // Apply pagination to filtered results in memory
                int skip = (currentPage - 1) * ItemsPerPage;
                var pagedItems = FilteredMatHangs.Skip(skip).Take(ItemsPerPage);
                DanhSachMatHang = [.. pagedItems];
            }
        }

        private async Task OpenSearchMatHangWindow()
        {
            SelectedMatHang = null!;

            var traCuuMatHangWindow = _serviceProvider.GetRequiredService<TraCuuMatHangWindow>();

            if (traCuuMatHangWindow.DataContext is TraCuuMatHangWindowViewModel viewModel)
            {
                viewModel.SearchCompleted += async (sender, searchResults) =>
                {
                    if (searchResults.Count > 0)
                    {
                        FilteredMatHangs = searchResults;
                        TotalPages = (int)Math.Ceiling((double)FilteredMatHangs.Count / ItemsPerPage);
                    }
                    else
                    {
                        FilteredMatHangs.Clear();
                        TotalPages = await _matHangService.GetTotalPages();
                    }
                    CurrentPage = "1";
                    _ = UpdatePagination();
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
