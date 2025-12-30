using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.QuanViews;

namespace QuanLyDaiLy.ViewModels.QuanViewModels
{
    public partial class ChinhSuaQuanViewModel :
        ObservableObject,
        IRecipient<SelectedIdMessage>
    {
        private readonly IQuanService _quanService;
        private int _quanId;

        public ChinhSuaQuanViewModel(IQuanService quanService)
        {
            _quanService = quanService;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(SelectedIdMessage message)
        {
            _quanId = message.Value;
            // Load data
            _ = LoadDataAsync();
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maQuan = "";

        [ObservableProperty]
        private string _tenQuan = "";

        private async Task LoadDataAsync()
        {
            try
            {
                var quan = await _quanService.GetQuanById(_quanId);
                MaQuan = quan.MaQuan.ToString();
                TenQuan = quan.TenQuan;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<CapNhatQuanWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task EditQuan()
        {
            if (string.IsNullOrWhiteSpace(TenQuan))
            {
                MessageBox.Show("Tên quận không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingQuan = await _quanService.GetQuanById(_quanId);

                existingQuan.TenQuan = TenQuan;

                await _quanService.UpdateQuan(existingQuan);

                MessageBox.Show("Cập nhật quận thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật quận: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}