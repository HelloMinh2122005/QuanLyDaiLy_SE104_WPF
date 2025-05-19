using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using QuanLyDaiLy.Messages;
using QuanLyDaiLy.Models;
using QuanLyDaiLy.Services;
using QuanLyDaiLy.Views.LoaiDaiLyViews;

namespace QuanLyDaiLy.ViewModels.LoaiDaiLyViewModels
{
    public partial class CapNhatLoaiDaiLyViewModel :
        ObservableObject,
        IRecipient<SelectedIdMessage>
    {
        private readonly ILoaiDaiLyService _loaiDaiLyService;
        private int _loaiDaiLyId;

        public CapNhatLoaiDaiLyViewModel(ILoaiDaiLyService loaiDaiLyService)
        {
            _loaiDaiLyService = loaiDaiLyService;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public void Receive(SelectedIdMessage message)
        {
            _loaiDaiLyId = message.Value;
            // Load data
            _ = LoadDataAsync();
        }

        // Binding properties with ObservableProperty attribute
        [ObservableProperty]
        private string _maLoaiDaiLy = "";

        [ObservableProperty]
        private string _tenLoaiDaiLy = "";

        [ObservableProperty]
        private string _noToiDa = "";

        private async Task LoadDataAsync()
        {
            try
            {
                var loaiDaiLy = await _loaiDaiLyService.GetLoaiDaiLyById(_loaiDaiLyId);
                MaLoaiDaiLy = loaiDaiLy.MaLoaiDaiLy.ToString();
                TenLoaiDaiLy = loaiDaiLy.TenLoaiDaiLy;
                NoToiDa = loaiDaiLy.NoToiDa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void CloseWindow()
        {
            WeakReferenceMessenger.Default.Send(new DataReloadMessage());
            Application.Current.Windows.OfType<CapNhatLoaiDaiLyWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private async Task CapNhatLoaiDaiLy()
        {
            if (string.IsNullOrWhiteSpace(TenLoaiDaiLy))
            {
                MessageBox.Show("Tên loại đại lý không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(NoToiDa))
            {
                MessageBox.Show("Nợ tối đa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var existingLoaiDaiLy = await _loaiDaiLyService.GetLoaiDaiLyById(_loaiDaiLyId);

                existingLoaiDaiLy.TenLoaiDaiLy = TenLoaiDaiLy;
                existingLoaiDaiLy.NoToiDa = long.Parse(NoToiDa);

                await _loaiDaiLyService.UpdateLoaiDaiLy(existingLoaiDaiLy);

                MessageBox.Show("Cập nhật loại đại lý thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật loại đại lý: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}