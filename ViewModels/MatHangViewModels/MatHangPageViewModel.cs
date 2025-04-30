using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.MatHangViews;

namespace QuanLyDaiLy.ViewModels.MatHangViewModels
{
    public partial class MatHangPageViewModel : ObservableObject, IRecipient<SearchCompletedMessage<MatHang>>
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

            // Only keep the parameterized command that can't use RelayCommand attribute
            PageSelectionCommand = new RelayCommand<string>(SelectPage);

            WeakReferenceMessenger.Default.Register(this);

            _ = LoadDataAsync();
        }

        public void Receive(SearchCompletedMessage<MatHang> message)
        {
            var searchResults = message.Value;

            if (searchResults.Count > 0)
            {
                FilteredMatHangs = searchResults;
                TotalPages = (int)Math.Ceiling((double)FilteredMatHangs.Count / ItemsPerPage);
            }
            else
            {
                _ = LoadDataAsync();
            }
            CurrentPage = "1";
            _ = UpdatePagination();
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
                (GoToNextPageCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (GoToPreviousPageCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }


        // Page button content properties
        [ObservableProperty]
        private string _buttonContentFirst = "1";

        [ObservableProperty]
        private string _buttonContentSecond = "2";

        [ObservableProperty]
        private string _buttonContentThird = "3";

        [ObservableProperty]
        private string _buttonContentForth = "4";

        [ObservableProperty]
        private string _buttonContentFith = "5";

        // Page button command parameter properties
        [ObservableProperty]
        private string _buttonParamFirst = "1";

        [ObservableProperty]
        private string _buttonParamSecond = "2";

        [ObservableProperty]
        private string _buttonParamThird = "3";

        [ObservableProperty]
        private string _buttonParamForth = "4";

        [ObservableProperty]
        private string _buttonParamFith = "5";

        [ObservableProperty]
        private ObservableCollection<MatHang> _filteredMatHangs = [];

        [ObservableProperty]
        private ObservableCollection<MatHang> _danhSachMatHang = [];

        [ObservableProperty]
        private MatHang _selectedMatHang = null!;

        [ObservableProperty]
        private Style _buttonStyleFirst;

        [ObservableProperty]
        private Style _buttonStyleSecond;

        [ObservableProperty]
        private Style _buttonStyleThird;

        [ObservableProperty]
        private Style _buttonStyleForth;

        [ObservableProperty]
        private Style _buttonStyleFith;

        // Keep the RelayCommand<string> as it requires a parameter
        public ICommand PageSelectionCommand { get; }

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

        [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
        private void GoToNextPage()
        {
            if (int.TryParse(CurrentPage, out int current) && current < TotalPages)
            {
                CurrentPage = (current + 1).ToString();
            }
        }

        [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
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

        [RelayCommand]
        private async Task SearchMatHang()
        {
            SelectedMatHang = null!;

            var traCuuMatHangWindow = _serviceProvider.GetRequiredService<TraCuuMatHangWindow>();
            traCuuMatHangWindow.Show();
        }

        [RelayCommand]
        private void AddMatHang()
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

        [RelayCommand]
        private void EditMatHang()
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

        [RelayCommand]
        private void DeleteMatHang()
        {
            _ = DeleteMatHangAsync();
        }

        private async Task DeleteMatHangAsync()
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

        [RelayCommand]
        private async Task LoadData()
        {
            SelectedMatHang = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}