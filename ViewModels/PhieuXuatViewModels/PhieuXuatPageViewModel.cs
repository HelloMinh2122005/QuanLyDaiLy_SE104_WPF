using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.PhieuXuatViews;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public class PhieuXuatPageViewModel : INotifyPropertyChanged
    {
        private readonly IPhieuXuatService _phieuXuatService;
        private readonly Func<int, CapNhatPhieuXuatWindowViewModel> _capNhatPhieuXuatFactory;
        private readonly IServiceProvider _serviceProvider;

        public PhieuXuatPageViewModel(
            IPhieuXuatService phieuXuatService, 
            IServiceProvider serviceProvider,
            Func<int, CapNhatPhieuXuatWindowViewModel> capNhatPhieuXuatFactory
        ) {
            _phieuXuatService = phieuXuatService;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _capNhatPhieuXuatFactory = capNhatPhieuXuatFactory;

            // Initialize commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecuteAsync());
            AddPhieuXuatCommand = new RelayCommand(AddPhieuXuat);
            EditPhieuXuatCommand = new RelayCommand(EditPhieuXuat);
            DeletePhieuXuatCommand = new RelayCommand(DeletePhieuXuat);
            SearchPhieuXuatCommand = new RelayCommand(SearchPhieuXuat);

            // Load initial data
            _ = LoadDataAsync();
        }

        // Binding properties
        private ObservableCollection<PhieuXuat> _danhSachPhieuXuat = [];
        public ObservableCollection<PhieuXuat> DanhSachPhieuXuat
        {
            get => _danhSachPhieuXuat;
            set
            {
                _danhSachPhieuXuat = value;
                OnPropertyChanged();
            }
        }

        private PhieuXuat _selectedPhieuXuat = null!;
        public PhieuXuat SelectedPhieuXuat
        {
            get => _selectedPhieuXuat;
            set
            {
                _selectedPhieuXuat = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand AddPhieuXuatCommand { get; }
        public ICommand EditPhieuXuatCommand { get; }
        public ICommand DeletePhieuXuatCommand { get; }
        public ICommand SearchPhieuXuatCommand { get; } 

        // Command methods
        private async Task LoadDataAsync()
        {
            var list = await _phieuXuatService.GetAllPhieuXuat();
            DanhSachPhieuXuat = [.. list];
        }

        private async Task LoadDataExecuteAsync()
        {
            SelectedPhieuXuat = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddPhieuXuat()
        {
            SelectedPhieuXuat = null!;

            var addPhieuXuatWindow = _serviceProvider.GetRequiredService<ThemPhieuXuatWindow>();
            if (addPhieuXuatWindow.DataContext is ThemPhieuXuatWindowViewModel viewModel)
            {
                viewModel.DataChanged += async (sender, e) => await LoadDataAsync();
            }
            addPhieuXuatWindow.Show();
        }

        private void EditPhieuXuat()
        {
            if (SelectedPhieuXuat == null!)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất để chỉnh sửa!", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            try
            {
                var viewmodel = _capNhatPhieuXuatFactory(SelectedPhieuXuat.MaPhieuXuat);
                viewmodel.DataChanged += async (sender, e) => await LoadDataAsync();

                var window = new CapNhatPhieuXuatWindow(viewmodel);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa phiếu xuất: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void DeletePhieuXuat()
        {
            _ = ExecuteDeletePhieuXuat();
        }

        private async Task ExecuteDeletePhieuXuat()
        {
            if (SelectedPhieuXuat != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu xuất này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Call the service to delete the selected PhieuXuat
                    await _phieuXuatService.DeletePhieuXuat(SelectedPhieuXuat.MaPhieuXuat);
                    await LoadDataAsync();
                }
            }
        }

        private void SearchPhieuXuat()
        {
            SelectedPhieuXuat = null!;

            var searchPhieuXuat = _serviceProvider.GetRequiredService<TraCuuPhieuXuatWindow>();
            searchPhieuXuat.Show();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
