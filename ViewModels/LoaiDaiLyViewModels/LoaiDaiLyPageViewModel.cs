using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels.QuanViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Views.LoaiDaiLyViews;
using QuanLyDaiLy.Views.QuanViews;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public class LoaiDaiLyPageViewModel : INotifyPropertyChanged
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, CapNhatLoaiDaiLyViewModel> _chinhSuaLoaiDaiLyFactory;

        public LoaiDaiLyPageViewModel(
            ILoaiDaiLyService loaiDaiLyService,
            IServiceProvider serviceProvider,
            Func<int, CapNhatLoaiDaiLyViewModel> chinhSuaLoaiDaiLyFactory
        )
        {
            _loaiDaiLyService = loaiDaiLyService;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _chinhSuaLoaiDaiLyFactory = chinhSuaLoaiDaiLyFactory;

            SearchLoaiDaiLyCommand = new RelayCommand(OpenSearchQuanWindow);
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());
            AddLoaiDaiLyCommand = new RelayCommand(OpenAddLoaiDaiLyWindow);
            EditLoaiDaiLyCommand = new RelayCommand(OpenEditLoaiDaiLyWindow);
            DeleteLoaiDaiLyCommand = new RelayCommand(ExecuteDeleteLoaiDaiLy);

            _ = LoadData();
        }

        // Observable collection of LoaiDaiLy objects for binding to the DataGrid
        private ObservableCollection<LoaiDaiLy> _danhSachLoaiDaiLy = [];
        public ObservableCollection<LoaiDaiLy> DanhSachLoaiDaiLy
        {
            get => _danhSachLoaiDaiLy;
            set
            {
                _danhSachLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }

        private LoaiDaiLy _selectedLoaiDaiLy = new();
        public LoaiDaiLy SelectedLoaiDaiLy
        {
            get => _selectedLoaiDaiLy;
            set
            {
                _selectedLoaiDaiLy = value;
                OnPropertyChanged();
            }
        }



        public ICommand SearchLoaiDaiLyCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand AddLoaiDaiLyCommand { get; }
        public ICommand EditLoaiDaiLyCommand { get; }
        public ICommand DeleteLoaiDaiLyCommand { get; }
        
        private async Task LoadData()
        {
            var list = await _loaiDaiLyService.GetAllLoaiDaiLy();
            DanhSachLoaiDaiLy = [.. list];
        }

        private async Task LoadDataExecute()
        {
            SelectedLoaiDaiLy = null!;
            await LoadData();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenAddLoaiDaiLyWindow()
        {
            SelectedLoaiDaiLy = null!;
            try
            {
                var addLoaiDaiLyWindow = _serviceProvider.GetRequiredService<ThemLoaiDaiLyWindow>();
                if (addLoaiDaiLyWindow.DataContext is ThemLoaiDaiLyViewModel viewModel)
                {
                    viewModel.DataChanged += async (sender, e) => await LoadData();
                }
                addLoaiDaiLyWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenEditLoaiDaiLyWindow()
        {
            if (SelectedLoaiDaiLy == null! || string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy))
            {
                MessageBox.Show("Vui lòng chọn loại đại lý để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewModel = _chinhSuaLoaiDaiLyFactory(SelectedLoaiDaiLy.MaLoaiDaiLy);
                viewModel.DataChanged += async (sender, e) => await LoadData();

                var window = new CapNhatLoaiDaiLyWindow(viewModel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenSearchQuanWindow()
        {
            SelectedLoaiDaiLy = null!;

            var traCuuLoaiDaiLyWindow = _serviceProvider.GetRequiredService<TraCuuLoaiDaiLyWindow>();

            if (traCuuLoaiDaiLyWindow.DataContext is TraCuuLoaiDaiLyWindowViewModel viewModel)
            {
                viewModel.SearchCompleted += (sender, searchResults) =>
                {
                    if (searchResults.Count > 0)
                    {
                        DanhSachLoaiDaiLy = searchResults;
                    }
                };
            }

            traCuuLoaiDaiLyWindow.Show();
        }

        // Delete the selected LoaiDaiLy
        private void ExecuteDeleteLoaiDaiLy()
        {
            _ = DeleteLoaiDaiLyAsync();
        }
        private async Task DeleteLoaiDaiLyAsync()
        {
            if (SelectedLoaiDaiLy == null! || string.IsNullOrEmpty(SelectedLoaiDaiLy.TenLoaiDaiLy))
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
                        await LoadData();
                        MessageBox.Show("Đã xóa loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        await LoadData();
                        MessageBox.Show("Đã xóa loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
