using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.DonViTinhViews;

namespace QuanLyDaiLy.ViewModels.DonViTinhViewModels
{
    public class DonViTinhPageViewModel : INotifyPropertyChanged
    {
        private readonly IDonViTinhService _iDonViTinhService;
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<int, CapNhatDonViTinhPageViewModel> _chinhSuaDonViTinhFactory;

        public DonViTinhPageViewModel(
            IDonViTinhService iDonViTinhService,
            IServiceProvider serviceProvider,
            Func<int, CapNhatDonViTinhPageViewModel> chinhSuaDonViTinhFactory
           )
        {
            _iDonViTinhService = iDonViTinhService;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _chinhSuaDonViTinhFactory = chinhSuaDonViTinhFactory;

            // Initialize commands
            SearchDonViTinhCommand = new RelayCommand(OpenSearchDonViTinhWindow);
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecute());
            AddDonViTinhCommand = new RelayCommand(OpenAddDonViTinhWindow);
            EditDonViTinhCommand = new RelayCommand(OpenEditDonViTinhWindow);
            DeleteDonViTinhCommand = new RelayCommand(ExecuteDeleteDonViTinh);

            _ = LoadData();
        }

        // Binding properties
        private ObservableCollection<DonViTinh> _danhSachDonViTinh = [];

        public ObservableCollection<DonViTinh> DanhSachDonViTinh
        {
            get => _danhSachDonViTinh;
            set
            {
                _danhSachDonViTinh = value;
                OnPropertyChanged();
            }
        }

        private DonViTinh _selectedDonViTinh = new();
        public DonViTinh SelectedDonViTinh
        {
            get => _selectedDonViTinh;
            set
            {
                _selectedDonViTinh = value;
                OnPropertyChanged();
            }
        }

        // Commands 
        public ICommand SearchDonViTinhCommand { get; }
        public ICommand LoadDataCommand { get; }
        public ICommand AddDonViTinhCommand { get; }
        public ICommand EditDonViTinhCommand { get; }
        public ICommand DeleteDonViTinhCommand { get; }

        private async Task LoadData()
        {
            var list = await _iDonViTinhService.GetAllDonViTinh();
            DanhSachDonViTinh = [.. list];
            SelectedDonViTinh = null!;
        }

        private async Task LoadDataExecute()
        {
            await LoadData();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /// <summary>
        /// PHẦN CỦA THÀNH CẦN SỬA
        /// </summary>
        private void OpenSearchDonViTinhWindow()
        {
            SelectedDonViTinh = null!;
            MessageBox.Show("Tính năng tra cứu đơn vị tính đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// PHẦN CỦA THÀNH CẦN SỬA
        /// </summary>
        /// 


        private void OpenAddDonViTinhWindow()
        {
            SelectedDonViTinh = null!;
            try
            {
                var addDonViTinhWindow = _serviceProvider.GetRequiredService<ThemDonViTinhWindow>();
                if (addDonViTinhWindow.DataContext is ThemDonViTinhPageViewModel viewModel)
                {
                    viewModel.DataChanged += async (sender, e) => await LoadData();
                }
                addDonViTinhWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void OpenEditDonViTinhWindow()
        {
            if (SelectedDonViTinh == null || string.IsNullOrEmpty(SelectedDonViTinh.TenDonViTinh))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewModel = _chinhSuaDonViTinhFactory(SelectedDonViTinh.MaDonViTinh);
                viewModel.DataChanged += async (sender, e) => await LoadData();

                var window = new CapNhatDonViTinhWindow(viewModel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteDonViTinh()
        {
            _ = DeleteDonViTinhAsync();
        }

        private async Task DeleteDonViTinhAsync()
        {
            if (SelectedDonViTinh == null || string.IsNullOrEmpty(SelectedDonViTinh.TenDonViTinh))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (SelectedDonViTinh.DsMatHang.Count > 0)
                {
                    var result = MessageBox.Show(
                        $"Đơn vị tính '{SelectedDonViTinh.TenDonViTinh}' đang chứa {SelectedDonViTinh.DsMatHang.Count} mặt hàng. Bạn có chắc chắn muốn xóa đơn vị tính này?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _iDonViTinhService.DeleteDonViTinh(SelectedDonViTinh.MaDonViTinh);
                        await LoadData();
                        MessageBox.Show("Đã xóa đơn vị tính thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa đơn vị tính '{SelectedDonViTinh.TenDonViTinh}'?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await _iDonViTinhService.DeleteDonViTinh(SelectedDonViTinh.MaDonViTinh);
                        await LoadData();
                        MessageBox.Show("Đã xóa đơn vị tính thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa đơn vị tính: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
