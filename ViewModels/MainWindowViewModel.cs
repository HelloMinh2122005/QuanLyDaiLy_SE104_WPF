using System.Collections.ObjectModel;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Commands;
using QuanLyDaiLy.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using QuanLyDaiLy.Messages;

//using CommunityToolkit.Mvvm.Input;

namespace QuanLyDaiLy.ViewModels
{
    public partial class MainWindowViewModel :
        ObservableObject,
        IRecipient<SearchCompletedMessage<DaiLy>>,
        IRecipient<DataReloadMessage>
    {
        // Services
        private readonly IDaiLyService _dailyService;
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel(
            IDaiLyService dailyService,
            IServiceProvider serviceProvider
        )
        {
            _dailyService = dailyService;
            _serviceProvider = serviceProvider;

            WeakReferenceMessenger.Default.RegisterAll(this);

            _ = LoadDataAsync();
        }

        public void Receive(DataReloadMessage message)
        {
           _ = LoadDataAsync();
        }
        public void Receive(SearchCompletedMessage<DaiLy> message)
        {
            var searchResults = message.Value;

            if (searchResults.Count > 0)
            {
                DanhSachDaiLy = searchResults;
            }
            else
            {
                _ = LoadDataAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            var list = await _dailyService.GetAllDaiLy();
            DanhSachDaiLy = [.. list];
            SelectedDaiLy = null!;
        }

        #region Binding Properties
        [ObservableProperty]
        private ObservableCollection<DaiLy> _danhSachDaiLy = [];

        [ObservableProperty]
        private DaiLy _selectedDaiLy = null!;
        #endregion

        #region RelayCommand
        [RelayCommand]
        private void AddDaiLy()
        {
            SelectedDaiLy = null!;
            try
            {
                var hoSoDaiLyWindow = _serviceProvider.GetRequiredService<HoSoDaiLyWinDow>();
                hoSoDaiLyWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ thêm mặt hàng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        [RelayCommand]
        private void EditDaiLy()
        {
            if (SelectedDaiLy == null || string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
            {
                MessageBox.Show("Vui lòng chọn đại lý để chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var window = _serviceProvider.GetRequiredService<ChinhSuaDaiLyWindow>();
                window.Show();
                WeakReferenceMessenger.Default.Send(new SelectedIdMessage(SelectedDaiLy.MaDaiLy));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở cửa sổ chỉnh sửa đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteDaiLy()
        {
            if (SelectedDaiLy == null! || string.IsNullOrEmpty(SelectedDaiLy.TenDaiLy))
            {
                MessageBox.Show("Vui lòng chọn đại lý để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa đại lý '{SelectedDaiLy.TenDaiLy}'?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _dailyService.DeleteDaiLy(SelectedDaiLy.MaDaiLy);
                    MessageBox.Show("Đã xóa đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        [RelayCommand]
        private async Task SearchDaiLy()
        {
            SelectedDaiLy = null!;

            var traCuuDaiLyWindow = _serviceProvider.GetRequiredService<TraCuuDaiLyWindow>();
            traCuuDaiLyWindow.Show();
        }
        [RelayCommand]
        private async Task LoadData()
        {
            SelectedDaiLy = null!;
            await LoadDataAsync();
            MessageBox.Show("Tải lại danh sách thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}