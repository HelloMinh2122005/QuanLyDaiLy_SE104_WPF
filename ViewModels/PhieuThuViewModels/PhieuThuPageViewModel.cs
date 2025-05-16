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
using QuanLyDaiLy.Views.PhieuThuViews;

namespace QuanLyDaiLy.ViewModels.PhieuThuViewModels
{
    public partial class PhieuThuPageViewModel : 
        ObservableObject,
        IRecipient<SearchCompletedMessage<PhieuThu>>,
        IRecipient<DataReloadMessage>
    {
        // Services
        private readonly IPhieuThuService _phieuThuService;
        private readonly IServiceProvider _serviceProvider;

        private record struct PaginationButton(string Content, string Parameter, Style Style);

        public PhieuThuPageViewModel(
            IPhieuThuService phieuThuService,
            IServiceProvider serviceProvider
        )
        {
            _phieuThuService = phieuThuService;
            _serviceProvider = serviceProvider;
            
            PageSelectionCommand = new RelayCommand<string>(SelectPage);

            WeakReferenceMessenger.Default.RegisterAll(this);
            
            _ = LoadDataAsync();
        }

        public void Receive(DataReloadMessage message)
        {
            _ = LoadDataAsync();
        }
        public void Receive(SearchCompletedMessage<PhieuThu> message)
        {
            var searchResults = message.Value;

            if (searchResults.Count > 0)
            {
                FilteredPhieuThus = searchResults;
                TotalPages = (int)Math.Ceiling((double)FilteredPhieuThus.Count / ItemsPerPage);
                CurrentPage = "1";
                _ = UpdatePagination();
            }
            else
            {
                _ = LoadDataAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            try
            {
                FilteredPhieuThus.Clear();
                var list = await _phieuThuService.GetPhieuThuPage(0);
                DanhSachPhieuThu = [.. list];

                TotalPages = await _phieuThuService.GetTotalPages();
                UpdateButtonVisibility();

                // Initialize pagination
                CurrentPage = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Pagination
        public ICommand PageSelectionCommand { get; }

        private int TotalPages = 0;
        private const int VisibleButtons = 5;
        private const int ItemsPerPage = 20;

        // Method
        private void SelectPage(string pageNumber)
        {
            if (int.TryParse(pageNumber, out int page) && page >= 1 && page <= TotalPages)
            {
                CurrentPage = pageNumber;
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
            if (FilteredPhieuThus.Count == 0)
            {
                var items = await _phieuThuService.GetPhieuThuPage(currentPage - 1);
                DanhSachPhieuThu = [.. items];
            }
            else
            {
                // Apply pagination to filtered results in memory
                int skip = (currentPage - 1) * ItemsPerPage;
                var pagedItems = FilteredPhieuThus.Skip(skip).Take(ItemsPerPage);
                DanhSachPhieuThu = [.. pagedItems];
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
        #endregion

        #region Bindings Properties
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
        private Style _buttonStyleFirst;
        [ObservableProperty]
        private Style _buttonStyleSecond;
        [ObservableProperty]
        private Style _buttonStyleThird;
        [ObservableProperty]
        private Style _buttonStyleForth;
        [ObservableProperty]
        private Style _buttonStyleFith;
        [ObservableProperty]
        private ObservableCollection<PhieuThu> _filteredPhieuThus = [];
        [ObservableProperty]
        private ObservableCollection<PhieuThu> _danhSachPhieuThu = [];
        [ObservableProperty]
        private PhieuThu _selectedPhieuThu = null!;
        #endregion

        #region RelayCommand
        [RelayCommand]
        private void AddPhieuThu()
        {
            SelectedPhieuThu = null!;
            try
            {
                var addPhieuThuWindow = _serviceProvider.GetRequiredService<ThemPhieuThuWindow>();
                addPhieuThuWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm phiếu thu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        [RelayCommand]
        private async Task DeletePhieuThu()
        {
            if (SelectedPhieuThu == null! || string.IsNullOrEmpty(SelectedPhieuThu.MaPhieuThu.ToString()))
            {
                MessageBox.Show("Vui lòng chọn phiếu thu để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu thu này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _phieuThuService.DeletePhieuThu(SelectedPhieuThu.MaPhieuThu);
                    MessageBox.Show("Đã xóa phiếu thu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi xóa phiếu thu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task SearchPhieuThu()
        {
            SelectedPhieuThu = null!;

            var traCuuPhieuThuWindow = _serviceProvider.GetRequiredService<TraCuuPhieuThuTienWindow>();
            traCuuPhieuThuWindow.Show();
        }

        [RelayCommand]
        private async Task LoadData()
        {
            SelectedPhieuThu = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}
