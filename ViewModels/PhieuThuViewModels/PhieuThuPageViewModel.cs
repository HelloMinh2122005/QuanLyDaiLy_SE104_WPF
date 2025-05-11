using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels.PhieuThuViewModels;
using QuanLyDaiLy.Views;
using QuanLyDaiLy.Views.PhieuThuViews;
using RelayCommand = QuanLyDaiLy.Commands.RelayCommand;

namespace QuanLyDaiLy.ViewModels.PhieuThuViewModels
{
    public partial class PhieuThuPageViewModel : ObservableObject
    {
        private readonly IPhieuThuService _phieuThuService;
        private readonly IServiceProvider _serviceProvider;

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand AddPhieuThuCommand { get; }
        public ICommand DeletePhieuThuCommand { get; }
        public ICommand SearchPhieuThuCommand { get; }

        public PhieuThuPageViewModel(
            IPhieuThuService phieuThuService,
            IServiceProvider serviceProvider
        )
        {
            _phieuThuService = phieuThuService;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Load initial data
            _ = LoadDataAsync();

            // Initialize commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataExecuteAsync());
            AddPhieuThuCommand = new RelayCommand(AddPhieuThu);
            DeletePhieuThuCommand = new RelayCommand(DeletePhieuThu);
            SearchPhieuThuCommand = new RelayCommand(SearchPhieuThu);
        }

        [ObservableProperty]
        private ObservableCollection<PhieuThu> _danhSachPhieuThu = [];

        private PhieuThu _selectedPhieuThu = null!;
        public PhieuThu SelectedPhieuThu
        {
            get => _selectedPhieuThu;
            set
            {
                _selectedPhieuThu = value;
                OnPropertyChanged();
            }
        }
        private async Task LoadDataAsync()
        {
            var list = await _phieuThuService.GetAllPhieuThu();
            DanhSachPhieuThu = [.. list];
        }
        private async Task LoadDataExecuteAsync()
        {
            SelectedPhieuThu = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddPhieuThu()
        {
            SelectedPhieuThu = null!;

            var addPhieuThuWindow = _serviceProvider.GetRequiredService<ThemPhieuThuWindow>();
            if (addPhieuThuWindow.DataContext is ThemPhieuThuWindowViewModel viewModel)
            {
                viewModel.DataChanged += async (sender, e) => await LoadDataAsync();
            }
            addPhieuThuWindow.Show();
        }

        private void DeletePhieuThu()
        {
            _ = ExecuteDeletePhieuThu();
        }

        private async Task ExecuteDeletePhieuThu()
        {
            if (SelectedPhieuThu == null! || string.IsNullOrEmpty(SelectedPhieuThu.MaPhieuThu.ToString()))
            {
                MessageBox.Show("Vui lòng chọn phiếu thu để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu thu này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Call the service to delete the selected PhieuThu
                    await _phieuThuService.DeletePhieuThu(SelectedPhieuThu.MaPhieuThu);
                    await LoadDataAsync();
                    MessageBox.Show("Đã xóa phiếu thu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi xóa phiếu thu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchPhieuThu()
        {
            SelectedPhieuThu = null!;

            var traCuuPhieuThuWindow = _serviceProvider.GetRequiredService<TraCuuPhieuThuTienWindow>();

            if (traCuuPhieuThuWindow.DataContext is TraCuuPhieuThuWindowViewModel viewModel)
            {
                viewModel.SearchCompleted += (sender, searchResults) =>
                {
                    if (searchResults.Count > 0)
                    {
                        DanhSachPhieuThu = searchResults;
                    }
                };
            }

            traCuuPhieuThuWindow.Show();
        }
    }
}
