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
using QuanLyDaiLy.Views.LoaiDaiLyViews;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public partial class LoaiDaiLyPageViewModel :
        ObservableObject,
        IRecipient<SearchCompletedMessage<LoaiDaiLy>>,
        IRecipient<DataReloadMessage>
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IServiceProvider _serviceProvider;

        private int TotalPages = 0;
        private const int VisibleButtons = 5;
        private const int ItemsPerPage = 20;

        private record struct PaginationButton(string Content, string Parameter, Style Style);

        public LoaiDaiLyPageViewModel(
            ILoaiDaiLyService loaiDaiLyService,
            IServiceProvider serviceProvider
        )
        {
            _loaiDaiLyService = loaiDaiLyService;
            _serviceProvider = serviceProvider;

            // Only keep the parameterized command that can't use RelayCommand attribute
            PageSelectionCommand = new RelayCommand<string>(SelectPage);

            WeakReferenceMessenger.Default.RegisterAll(this);

            _ = LoadDataAsync();
        }

        public void Receive(SearchCompletedMessage<LoaiDaiLy> message)
        {
            var searchResults = message.Value;

            if (searchResults.Count > 0)
            {
                FilteredLoaiDaiLies = searchResults;
                TotalPages = (int)Math.Ceiling((double)FilteredLoaiDaiLies.Count / ItemsPerPage);
                CurrentPage = "1";
                _ = UpdatePagination();
            }
            else
            {
                _ = LoadDataAsync();
            }

        }

        public void Receive(DataReloadMessage message)
        {
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
        private ObservableCollection<LoaiDaiLy> _filteredLoaiDaiLies = [];

        [ObservableProperty]
        private ObservableCollection<LoaiDaiLy> _danhSachLoaiDaiLy = [];

        [ObservableProperty]
        private LoaiDaiLy _selectedLoaiDaiLy = null!;

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
                FilteredLoaiDaiLies.Clear();
                var list = await _loaiDaiLyService.GetLoaiDaiLyPage(0);
                DanhSachLoaiDaiLy = [.. list];

                TotalPages = await _loaiDaiLyService.GetTotalPages();
                UpdateButtonVisibility();

                // Initialize pagination
                CurrentPage = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (FilteredLoaiDaiLies.Count == 0)
            {
                // Load from service when no filter is applied
                var items = await _loaiDaiLyService.GetLoaiDaiLyPage(currentPage - 1);
                DanhSachLoaiDaiLy = [.. items];
            }
            else
            {
                // Apply pagination to filtered results in memory
                int skip = (currentPage - 1) * ItemsPerPage;
                var pagedItems = FilteredLoaiDaiLies.Skip(skip).Take(ItemsPerPage);
                DanhSachLoaiDaiLy = [.. pagedItems];
            }
        }

        [RelayCommand]
        private async Task SearchLoaiDaiLy()
        {
            SelectedLoaiDaiLy = null!;

            var traCuuLoaiDaiLyWindow = _serviceProvider.GetRequiredService<TraCuuLoaiDaiLyWindow>();
            traCuuLoaiDaiLyWindow.Show();
        }

        [RelayCommand]
        private void AddLoaiDaiLy()
        {
            try
            {
                var addLoaiDaiLyWindow = _serviceProvider.GetRequiredService<ThemLoaiDaiLyWindow>();
                addLoaiDaiLyWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SelectedLoaiDaiLy = null!;
        }

        [RelayCommand]
        private void EditLoaiDaiLy()
        {
            if (SelectedLoaiDaiLy == null || string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy))
            {
                MessageBox.Show("Vui lòng chọn loại đại lý để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var window = _serviceProvider.GetRequiredService<CapNhatLoaiDaiLyWindow>();
                window.Show();
                WeakReferenceMessenger.Default.Send(new SelectedIdMessage(SelectedLoaiDaiLy.MaLoaiDaiLy));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteLoaiDaiLy()
        {
            if (SelectedLoaiDaiLy == null)
            {
                MessageBox.Show("Vui lòng chọn loại đại lý để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var soLuongDaiLyTrongLoaiDaiLy = SelectedLoaiDaiLy.DsDaiLy.Count;
                if (soLuongDaiLyTrongLoaiDaiLy > 0)
                {
                    var result = MessageBox.Show(
                        $"Loại đại lý '{SelectedLoaiDaiLy.TenLoaiDaiLy}' đang chứa {soLuongDaiLyTrongLoaiDaiLy} đại lý. Bạn có chắc chắn muốn xóa loại đại lý này?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await _loaiDaiLyService.DeleteLoaiDaiLy(SelectedLoaiDaiLy.MaLoaiDaiLy);
                        MessageBox.Show("Đã xóa loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadDataAsync();
                    }
                }
                else
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa loại đại lý '{SelectedLoaiDaiLy.TenLoaiDaiLy}'?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await _loaiDaiLyService.DeleteLoaiDaiLy(SelectedLoaiDaiLy.MaLoaiDaiLy);
                        MessageBox.Show("Đã xóa loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadDataAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task LoadData()
        {
            SelectedLoaiDaiLy = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
