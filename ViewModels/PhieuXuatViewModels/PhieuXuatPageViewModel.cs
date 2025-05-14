using System.Collections.ObjectModel;
//using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.ViewModels.QuanViewModels;
using QuanLyDaiLy.Views.PhieuXuatViews;
using QuanLyDaiLy.Views.QuanViews;

namespace QuanLyDaiLy.ViewModels.PhieuXuatViewModels
{
    public partial class PhieuXuatPageViewModel :
        ObservableObject,
        //IRecipient<SearchCompletedMessage<PhieuXuat>>,
        IRecipient<DataReloadMessage>
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
            _serviceProvider = serviceProvider ;
            _capNhatPhieuXuatFactory = capNhatPhieuXuatFactory;

            WeakReferenceMessenger.Default.RegisterAll(this);
            // Load initial data
            _ = LoadDataAsync();
        }

        public void Receive(DataReloadMessage message)
        {
            _ = LoadDataAsync();
        }

        // Binding properties
        [ObservableProperty]
        private ObservableCollection<PhieuXuat> _danhSachPhieuXuat = [];

        [ObservableProperty]
        private PhieuXuat _selectedPhieuXuat = null!;
        
        
        // Command methods
        private async Task LoadDataAsync()
        {
            var list = await _phieuXuatService.GetAllPhieuXuat();
            DanhSachPhieuXuat = [.. list];
        }

        [RelayCommand]
        private async Task LoadDataExecuteAsync()
        {
            SelectedPhieuXuat = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        [RelayCommand]
        private void AddPhieuXuat()
        {
            SelectedPhieuXuat = null!;

            var addPhieuXuatWindow = _serviceProvider.GetRequiredService<ThemPhieuXuatWindow>();
            addPhieuXuatWindow.Show();
        }

        [RelayCommand]
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

                var window = new CapNhatPhieuXuatWindow(viewmodel);
                window.Show();
                WeakReferenceMessenger.Default.Send(new SelectedIdMessage(SelectedPhieuXuat.MaPhieuXuat));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa phiếu xuất: {ex.Message}", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void DeletePhieuXuat()
        {
            _ = ExecuteDeletePhieuXuat();
        }
        
        private async Task ExecuteDeletePhieuXuat()
        {
            if (SelectedPhieuXuat == null!)
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất để xoá!", "Thông báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

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

        [RelayCommand]
        private void SearchPhieuXuat()
        {
            SelectedPhieuXuat = null!;

            var traCuuPhieuXuatWindow = _serviceProvider.GetRequiredService<TraCuuPhieuXuatWindow>();

            if (traCuuPhieuXuatWindow.DataContext is TraCuuPhieuXuatWindowViewModel viewModel)
            {
                viewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(viewModel.SearchResults) && viewModel.SearchResults != null)
                    {
                        DanhSachPhieuXuat = viewModel.SearchResults;
                    }
                };
            }

            traCuuPhieuXuatWindow.Show();
        }
    }
}