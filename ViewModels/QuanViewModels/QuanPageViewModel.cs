using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;
using QuanLyDaiLy.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public class QuanPageViewModel : INotifyPropertyChanged
    {
        private readonly IQuanService _quanService;
        private readonly IServiceProvider _serviceProvider;

        public QuanPageViewModel(
            IQuanService quanService,
            IServiceProvider serviceProvider
        ) {
            _quanService = quanService;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            SearchQuanCommand = new RelayCommand(OpenSearchQuanWindow);
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());
            AddQuanCommand = new RelayCommand(OpenAddQuanWindow);
            EditQuanCommand = new RelayCommand(OpenEditQuanWindow);
            DeleteQuanCommand = new RelayCommand(ExecuteDeleteQuan);

            _ = LoadData();
        }

        // Observable collection of Quan objects for binding to the DataGrid
        private ObservableCollection<Quan> _danhSachQuan = [];
        public ObservableCollection<Quan> DanhSachQuan
        {
            get => _danhSachQuan;
            set
            {
                _danhSachQuan = value;
                OnPropertyChanged();
            }
        }

        private Quan _selectedQuan = new();
        public Quan SelectedQuan
        {
            get => _selectedQuan;
            set
            {
                _selectedQuan = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchQuanCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand AddQuanCommand { get; }
        public ICommand EditQuanCommand { get; }
        public ICommand DeleteQuanCommand { get; }

        private async Task LoadData()
        {
            var list = await _quanService.GetAllQuan();
            DanhSachQuan = [.. list];
        }

        private async Task LoadDataExecute()
        {
            await LoadData();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenSearchQuanWindow()
        {
            MessageBox.Show("Tính năng tra cứu quận đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Open add window
        private void OpenAddQuanWindow()
        {
            try
            {
                var addQuanWindow = _serviceProvider.GetRequiredService<ThemQuanWindow>();
                if (addQuanWindow.DataContext is ThemQuanViewModel viewModel)
                {
                    viewModel.DataChanged += async (sender, e) => await LoadData();
                }
                addQuanWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenEditQuanWindow()
        {
            if (SelectedQuan == null || SelectedQuan.MaQuan == 0)
            {
                MessageBox.Show("Vui lòng chọn quận để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Implementation depends on your UI flow
                // var editQuanWindow = _serviceProvider.GetRequiredService<ChinhSuaQuanWindow>();
                // if (editQuanWindow.DataContext is ChinhSuaQuanViewModel viewModel)
                // {
                //     viewModel.QuanId = SelectedQuan.MaQuan;
                //     viewModel.DataChanged += async (sender, e) => await LoadData();
                // }
                // editQuanWindow.Show();

                MessageBox.Show("Tính năng sửa quận đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Delete the selected Quan
        private async void ExecuteDeleteQuan()
        {
            if (SelectedQuan == null || SelectedQuan.MaQuan == 0)
            {
                MessageBox.Show("Vui lòng chọn quận để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                int soLuongDaiLy = await _quanService.GetSoLuongDaiLyTrongQuan(SelectedQuan.MaQuan);
                if (soLuongDaiLy > 0)
                {
                    var result = MessageBox.Show(
                        $"Quận '{SelectedQuan.TenQuan}' đang chứa {soLuongDaiLy} đại lý. Bạn có chắc chắn muốn xóa quận này?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _quanService.DeleteQuan(SelectedQuan.MaQuan);
                        await LoadData();
                        MessageBox.Show("Đã xóa quận thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa quận '{SelectedQuan.TenQuan}'?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await _quanService.DeleteQuan(SelectedQuan.MaQuan);
                        await LoadData();
                        MessageBox.Show("Đã xóa quận thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}